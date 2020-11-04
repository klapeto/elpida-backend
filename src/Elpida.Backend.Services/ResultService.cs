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
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private readonly ICpuRepository _cpuRepository;

		private readonly IIdProvider _idProvider;

		private readonly IResultsRepository _resultsRepository;

		public ResultService(IResultsRepository resultsRepository, ICpuRepository cpuRepository, IIdProvider idProvider)
		{
			_resultsRepository = resultsRepository ?? throw new ArgumentNullException(nameof(resultsRepository));
			_cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
			_idProvider = idProvider ?? throw new ArgumentNullException(nameof(idProvider));
			;
		}

		private static IReadOnlyDictionary<string, LambdaExpression> ResultProjectionExpressions { get; } =
			new Dictionary<string, LambdaExpression>
			{
				[Filter.TypeMap[Filter.Type.CpuCores].ToLowerInvariant()] =
					GetExpression(model => model.System.Topology.TotalPhysicalCores),
				[Filter.TypeMap[Filter.Type.CpuLogicalCores].ToLowerInvariant()] =
					GetExpression(model => model.System.Topology.TotalLogicalCores),
				[Filter.TypeMap[Filter.Type.CpuFrequency].ToLowerInvariant()] =
					GetExpression(model => model.System.Cpu.Frequency),
				[Filter.TypeMap[Filter.Type.MemorySize].ToLowerInvariant()] =
					GetExpression(model => model.System.Memory.TotalSize),
				[Filter.TypeMap[Filter.Type.Timestamp].ToLowerInvariant()] = GetExpression(model => model.TimeStamp),
				[Filter.TypeMap[Filter.Type.Name].ToLowerInvariant()] = GetExpression(model => model.Result.Name),
				[Filter.TypeMap[Filter.Type.CpuBrand].ToLowerInvariant()] =
					GetExpression(model => model.System.Cpu.Brand),
				[Filter.TypeMap[Filter.Type.CpuVendor].ToLowerInvariant()] =
					GetExpression(model => model.System.Cpu.Vendor),
				[Filter.TypeMap[Filter.Type.OsCategory].ToLowerInvariant()] =
					GetExpression(model => model.System.Os.Category),
				[Filter.TypeMap[Filter.Type.OsName].ToLowerInvariant()] = GetExpression(model => model.System.Os.Name),
				[Filter.TypeMap[Filter.Type.OsVersion].ToLowerInvariant()] =
					GetExpression(model => model.System.Os.Version)
			};

		#region IResultsService Members

		public async Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			if (resultDto == null)
			{
				throw new ArgumentNullException(nameof(resultDto));
			}

			resultDto.TimeStamp = DateTime.UtcNow;

			var cpuId = await AssignCpu(resultDto.System.Cpu, cancellationToken);
			var topologyId = await AssignTopology(cpuId, resultDto.System.Topology, cancellationToken);

			return await _resultsRepository.CreateAsync(resultDto.ToModel(_idProvider.GetForResult(resultDto), cpuId, topologyId), cancellationToken);
		}

		public async Task<ResultDto> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be empty", nameof(id));
			}

			var model = await _resultsRepository.GetSingleAsync(id, cancellationToken);
			if (model == null)
			{
				throw new NotFoundException(id);
			}

			return model.ToDto();
		}

		public async Task<PagedResult<ResultPreviewDto>> GetPagedAsync(QueryRequest queryRequest,
			CancellationToken cancellationToken)
		{
			if (queryRequest == null)
			{
				throw new ArgumentNullException(nameof(queryRequest));
			}

			var expressionBuilder = new QueryExpressionBuilder(ResultProjectionExpressions);

			var result = await _resultsRepository.GetAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				queryRequest.Descending,
				expressionBuilder.GetOrderBy<ResultProjection>(queryRequest),
				expressionBuilder.Build<ResultProjection>(queryRequest.Filters),
				queryRequest.PageRequest.TotalCount == 0,
				cancellationToken);

			queryRequest.PageRequest.TotalCount = result.TotalCount;

			return new PagedResult<ResultPreviewDto>(result.Items.Select(m => m.ToDto()).ToList(),
				queryRequest.PageRequest);
		}

		public Task ClearResultsAsync(CancellationToken cancellationToken)
		{
			return _resultsRepository.DeleteAllAsync(cancellationToken);
		}

		#endregion

		private static LambdaExpression GetExpression<T>(Expression<Func<ResultProjection, T>> baseExp)
		{
			// Dirty hack to prevent boxing of values
			return baseExp;
		}

		private async Task<string> AssignCpu(CpuDto cpuDto, CancellationToken cancellationToken)
		{
			var id = _idProvider.GetForCpu(cpuDto);
			var cpuModel = await _cpuRepository.GetCpuByIdAsync(id, cancellationToken);
			if (cpuModel == null)
			{
				cpuModel = cpuDto.ToModel(id);
				await _cpuRepository.CreateCpuAsync(cpuModel, cancellationToken);
			}

			return id;
		}


		private async Task<string> AssignTopology(string cpuId, TopologyDto topologyDto,
			CancellationToken cancellationToken)
		{
			var id = _idProvider.GetForTopology(cpuId, topologyDto);
			var topologyModel = await _cpuRepository.GetTopologyByIdAsync(id, cancellationToken);
			if (topologyModel == null)
			{
				topologyModel = topologyDto.ToModel(id);
				await _cpuRepository.CreateTopologyAsync(topologyModel, cancellationToken);
			}

			return id;
		}
	}
}