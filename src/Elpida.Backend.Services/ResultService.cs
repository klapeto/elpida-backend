/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Extensions.Result;
using Elpida.Backend.Services.Extensions.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private readonly ICpuRepository _cpuRepository;
		private readonly IResultsRepository _resultsRepository;
		private readonly ITopologyRepository _topologyRepository;
		private readonly IBenchmarkRepository _benchmarkRepository;

		public ResultService(IResultsRepository resultsRepository,
			ICpuRepository cpuRepository,
			ITopologyRepository topologyRepository,
			IBenchmarkRepository benchmarkRepository)
		{
			_resultsRepository = resultsRepository;
			_cpuRepository = cpuRepository;
			_benchmarkRepository = benchmarkRepository;
			_topologyRepository = topologyRepository;
		}

		private static IReadOnlyDictionary<string, LambdaExpression> CpuExpressions { get; } =
			new Dictionary<string, LambdaExpression>
			{
				[FilterHelpers.TypeMap[FilterHelpers.Type.CpuBrand]] = GetResultExpression(model => model.Topology.Cpu.Brand),
				[FilterHelpers.TypeMap[FilterHelpers.Type.CpuVendor]] = GetResultExpression(model => model.Topology.Cpu.Vendor),
				[FilterHelpers.TypeMap[FilterHelpers.Type.CpuFrequency]] = GetResultExpression(model => model.Topology.Cpu.Frequency),
			};

		private static IReadOnlyDictionary<string, LambdaExpression> TopologyExpressions { get; } =
			new Dictionary<string, LambdaExpression>
			{
				[FilterHelpers.TypeMap[FilterHelpers.Type.CpuCores]] =
					GetResultExpression(model => model.Topology.TotalPhysicalCores),
				[FilterHelpers.TypeMap[FilterHelpers.Type.CpuLogicalCores]] =
					GetResultExpression(model => model.Topology.TotalLogicalCores),
			};

		private static IReadOnlyDictionary<string, LambdaExpression> ResultExpressions { get; } =
			new Dictionary<string, LambdaExpression>
			{
				[FilterHelpers.TypeMap[FilterHelpers.Type.MemorySize]] = GetResultExpression(model => model.MemorySize),
				[FilterHelpers.TypeMap[FilterHelpers.Type.Timestamp]] = GetResultExpression(model => model.TimeStamp),
				["startTime".ToLowerInvariant()] = GetResultExpression(model => model.TimeStamp),
				["endTime".ToLowerInvariant()] = GetResultExpression(model => model.TimeStamp),
				[FilterHelpers.TypeMap[FilterHelpers.Type.Name].ToLowerInvariant()] = GetResultExpression(model => model.Benchmark.Name),
				[FilterHelpers.TypeMap[FilterHelpers.Type.OsCategory]] = GetResultExpression(model => model.OsCategory),
				[FilterHelpers.TypeMap[FilterHelpers.Type.OsName]] = GetResultExpression(model => model.OsName),
				[FilterHelpers.TypeMap[FilterHelpers.Type.OsVersion]] = GetResultExpression(model => model.OsVersion),
			};

		private static IReadOnlyDictionary<string, LambdaExpression> ResultProjectionExpressions { get; } =
			ResultExpressions.Concat(CpuExpressions)
				.Concat(TopologyExpressions)
				.ToDictionary(x => x.Key, x => x.Value);

		#region IResultsService Members

		public async Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			var benchmark = await _benchmarkRepository.GetSingleAsync(t => t.Uuid == resultDto.Result.Uuid, cancellationToken);
			if (benchmark == null)
			{
				throw new NotFoundException($"The benchmark '{resultDto.Result.Name}' was not found in database.",string.Empty);
			}
			
			resultDto.TimeStamp = DateTime.UtcNow;

			var cpu = await AssignCpu(resultDto.System.Cpu, cancellationToken);
			var topology = await AssignTopology(cpu, resultDto.System.Topology, cancellationToken);
			
			var resultModel = resultDto.ToModel(benchmark, topology, resultDto.Result.TaskResults.Select(r =>
			{
				var task = benchmark.Tasks.FirstOrDefault(t => t.Uuid == r.Uuid);
				if (task == null) throw new ConflictException($"The task '{r.Name}' was not found in the benchmark '{benchmark.Name}' tasks", string.Empty);
				return r.ToModel(task);
			}).ToList());

			var result = await _resultsRepository.CreateAsync(resultModel, cancellationToken);
			
			await _resultsRepository.SaveChangesAsync(cancellationToken);
			await _cpuRepository.SaveChangesAsync(cancellationToken);
			await _topologyRepository.SaveChangesAsync(cancellationToken);
			
			return result.Id.ToString();
		}

		public async Task<ResultDto> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be empty", nameof(id));
			}

			if (!long.TryParse(id, out var idL))
			{
				throw new ArgumentException("Id cannot must be a number", nameof(id));
			}

			var resultModel = await _resultsRepository.GetSingleAsync(idL, cancellationToken);
			
			if (resultModel == null)
			{
				throw new NotFoundException(id);
			}

			return resultModel.ToDto();
		}

		public async Task<PagedResult<ResultPreviewDto>> GetPagedAsync(QueryRequest queryRequest,
			CancellationToken cancellationToken)
		{
			if (queryRequest == null)
			{
				throw new ArgumentNullException(nameof(queryRequest));
			}

			var expressionBuilder = new QueryExpressionBuilder(ResultProjectionExpressions);

			var result = await _resultsRepository.GetPagedPreviewsAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				queryRequest.Descending,
				expressionBuilder.GetOrderBy<ResultModel>(queryRequest),
				expressionBuilder.Build<ResultModel>(queryRequest.Filters),
				queryRequest.PageRequest.TotalCount == 0,
				cancellationToken);

			queryRequest.PageRequest.TotalCount = result.TotalCount;

			return new PagedResult<ResultPreviewDto>(result.Items.Select(m => m.ToDto()).ToList(),
				queryRequest.PageRequest);
		}

		#endregion

		private static LambdaExpression GetResultExpression<T>(Expression<Func<ResultModel, T>> baseExp)
		{
			// Dirty hack to prevent boxing of values
			return baseExp;
		}

		private async Task<CpuModel> AssignCpu(CpuDto cpuDto, CancellationToken cancellationToken)
		{
			var additionalInfo = cpuDto.ToModel(0).AdditionalInfo;
			
			var cpuModel = await _cpuRepository.GetSingleAsync(model => 
				model.Vendor == cpuDto.Vendor 
				&& model.Brand == cpuDto.Brand 
				&& model.AdditionalInfo == additionalInfo, cancellationToken);
			if (cpuModel == null)
			{
				cpuModel = cpuDto.ToModel(0);
				return await _cpuRepository.CreateAsync(cpuModel, cancellationToken);
			}

			return cpuModel;
		}

		private static string GetHash(string str)
		{
			using var md5 = MD5.Create();
			using var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
			
			return md5.ComputeHash(ms).ToHexString();
		}
		
		private async Task<TopologyModel> AssignTopology(CpuModel cpu, TopologyDto topologyDto,
			CancellationToken cancellationToken)
		{
			var topologyRoot = JsonConvert.SerializeObject(topologyDto.Root);
			var topologyHash = GetHash(topologyRoot);
			
			var topologyModel = await _topologyRepository.GetSingleAsync(t =>
				t.CpuId == cpu.Id
				&& t.TopologyHash == topologyHash, 
				cancellationToken);
			if (topologyModel == null)
			{
				topologyModel = topologyDto.ToModel(cpu, topologyRoot, topologyHash, 0);
				await _topologyRepository.CreateAsync(topologyModel, cancellationToken);
			}

			return topologyModel;
		}
	}
}