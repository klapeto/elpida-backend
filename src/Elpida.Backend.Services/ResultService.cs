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
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private static readonly MethodInfo RegexIsMatch =
			typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string), typeof(string)});

		private static readonly IReadOnlyDictionary<string, Func<Expression, Expression, Expression>>
			ComparisonExpressionsFactories = new Dictionary<string, Func<Expression, Expression, Expression>>
			{
				[Filter.ComparisonMap[Filter.Comparison.Contains]] =
					(left, right) => Expression.Call(RegexIsMatch, left, right),
				[Filter.ComparisonMap[Filter.Comparison.Equal]] = Expression.Equal,
				[Filter.ComparisonMap[Filter.Comparison.Greater]] = Expression.GreaterThan,
				[Filter.ComparisonMap[Filter.Comparison.GreaterEqual]] = Expression.GreaterThanOrEqual,
				[Filter.ComparisonMap[Filter.Comparison.Less]] = Expression.LessThan,
				[Filter.ComparisonMap[Filter.Comparison.LessEqual]] = Expression.LessThanOrEqual
			};

		private static readonly IReadOnlyDictionary<string, LambdaExpression> ModelExpressions =
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

		private static readonly uint[] Lookup32 = CreateLookup32();
		private readonly ICpuRepository _cpuRepository;

		private readonly IResultsRepository _resultsRepository;

		public ResultService(IResultsRepository resultsRepository, ICpuRepository cpuRepository)
		{
			_resultsRepository = resultsRepository ?? throw new ArgumentNullException(nameof(resultsRepository));
			_cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
		}

		#region IResultsService Members

		public async Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			if (resultDto == null)
			{
				throw new ArgumentNullException(nameof(resultDto));
			}

			resultDto.TimeStamp = DateTime.UtcNow;

			var cpuHash = await AssignCpu(resultDto.System.Cpu, cancellationToken);
			var topologyHash = await AssignTopology(cpuHash, resultDto.System.Topology, cancellationToken);

			return await _resultsRepository.CreateAsync(resultDto.ToModel(cpuHash, topologyHash), cancellationToken);
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

			var result = await _resultsRepository.GetAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				queryRequest.Descending,
				GetOrderBy(queryRequest),
				GetQueryFilters(queryRequest),
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

		private async Task<string> AssignCpu(CpuDto cpuDto, CancellationToken cancellationToken)
		{
			var cpuHash = CalculateCpuHash(cpuDto);
			var cpuModel = await _cpuRepository.GetCpuByHashAsync(cpuHash, cancellationToken);
			if (cpuModel == null)
			{
				cpuModel = cpuDto.ToModel();
				cpuModel.Hash = cpuHash;
				await _cpuRepository.CreateCpuAsync(cpuModel, cancellationToken);
			}

			return cpuHash;
		}

		private static uint[] CreateLookup32()
		{
			var result = new uint[256];
			for (var i = 0; i < 256; i++)
			{
				var s = i.ToString("X2");
				result[i] = s[0] + ((uint) s[1] << 16);
			}

			return result;
		}

		private static string ByteArrayToHexViaLookup32(IReadOnlyList<byte> bytes)
		{
			//https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
			var lookup32 = Lookup32;
			var result = new char[bytes.Count * 2];
			for (var i = 0; i < bytes.Count; i++)
			{
				var val = lookup32[bytes[i]];
				result[2 * i] = (char) val;
				result[2 * i + 1] = (char) (val >> 16);
			}

			return new string(result);
		}

		private async Task<string> AssignTopology(string cpuHash, TopologyDto topologyDto,
			CancellationToken cancellationToken)
		{
			var topologyHash = CalculateTopologyHash(cpuHash, topologyDto);
			var topologyModel = await _cpuRepository.GetTopologyByHashAsync(topologyHash, cancellationToken);
			if (topologyModel == null)
			{
				topologyModel = topologyDto.ToModel();
				topologyModel.Hash = topologyHash;
				await _cpuRepository.CreateTopologyAsync(topologyModel, cancellationToken);
			}

			return topologyHash;
		}

		private static string CalculateTopologyHash(string cpuHash, TopologyDto topologyDto)
		{
			using var stream = new MemoryStream();

			var formatter = new BinaryFormatter();
			formatter.Serialize(stream, topologyDto);

			using var md5 = MD5.Create();
			return $"{cpuHash}_{ByteArrayToHexViaLookup32(md5.ComputeHash(stream))}";
		}

		private static string CalculateCpuHash(CpuDto cpuDto)
		{
			return
				$"{cpuDto.Vendor}_{cpuDto.Brand}_{string.Join('_', cpuDto.AdditionalInfo.ToArray().OrderBy(c => c.Key).Select(c => c.Value))}";
		}

		private static LambdaExpression GetExpression<T>(Expression<Func<ResultProjection, T>> baseExp)
		{
			// Dirty hack to prevent boxing of values
			return baseExp;
		}

		private static void AddFilter(ICollection<Expression<Func<ResultProjection, bool>>> accumulator,
			QueryInstance instance, LambdaExpression fieldPart)
		{
			if (instance == null)
			{
				return;
			}

			Expression right = Expression.Constant(Convert.ChangeType(instance.Value, fieldPart.Body.Type));
			var left = fieldPart.Body;
			var parameters = fieldPart.Parameters;

			Expression middlePart;

			if (instance.Value is string str && !DateTime.TryParse(str, out _))
			{
				if (instance.Comp != null)
				{
					if (
						Filter.StringComparisons.Contains(instance.Comp) &&
						ComparisonExpressionsFactories.TryGetValue(instance.Comp, out var factory))
					{
						middlePart = factory(left, right);
					}
					else
					{
						throw new ArgumentException(
							$"String value filter comparison types can be :[{string.Join(",", Filter.StringComparisons.Select(s => s))}]");
					}
				}
				else
				{
					middlePart =
						ComparisonExpressionsFactories[Filter.ComparisonMap[Filter.Comparison.Contains]](left, right);
				}
			}
			else
			{
				if (instance.Comp != null)
				{
					if (Filter.NumberComparisons.Contains(instance.Comp) &&
					    ComparisonExpressionsFactories.TryGetValue(instance.Comp, out var factory))
					{
						middlePart = factory(left, right);
					}
					else
					{
						throw new ArgumentException(
							$"Numeric value filter comparison types can be :[{string.Join(",", Filter.NumberComparisons.Select(s => s))}]");
					}
				}
				else
				{
					middlePart =
						ComparisonExpressionsFactories[Filter.ComparisonMap[Filter.Comparison.Equal]](left, right);
				}
			}

			accumulator.Add(Expression.Lambda<Func<ResultProjection, bool>>(middlePart, parameters));
		}


		private static Expression<Func<ResultProjection, object>> GetOrderBy(QueryRequest queryRequest)
		{
			if (queryRequest.OrderBy == null)
			{
				return null;
			}

			var orderBy = queryRequest.OrderBy.ToLowerInvariant();

			if (ModelExpressions.TryGetValue(orderBy, out var strExpression))
			{
				return Expression.Lambda<Func<ResultProjection, object>>(
					Expression.Convert(strExpression.Body, typeof(object)), strExpression.Parameters);
			}

			throw new ArgumentException(
				$"OrderBy is not a valid order field. Can be: {string.Join(',', ModelExpressions.Keys)}");
		}

		private static IEnumerable<Expression<Func<ResultProjection, bool>>> GetQueryFilters(QueryRequest queryRequest)
		{
			if (queryRequest.Filters == null)
			{
				return null;
			}

			var returnList = new List<Expression<Func<ResultProjection, bool>>>();

			foreach (var filter in queryRequest.Filters)
			{
				if (ModelExpressions.TryGetValue(filter.Name.ToLowerInvariant(), out var expression))
				{
					AddFilter(returnList, filter, expression);
				}
			}

			return returnList;
		}
	}
}