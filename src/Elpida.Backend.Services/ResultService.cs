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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private static readonly MethodInfo StringContains =
			typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)});

		private static readonly IReadOnlyDictionary<string, Func<Expression, Expression, Expression>>
			ComparisonExpressionsFactories = new Dictionary<string, Func<Expression, Expression, Expression>>
			{
				[Filter.ComparisonMap[Filter.Comparison.Contains]] =
					(left, right) => Expression.Call(left, StringContains, right),
				[Filter.ComparisonMap[Filter.Comparison.Equal]] = Expression.Equal,
				[Filter.ComparisonMap[Filter.Comparison.Greater]] = Expression.GreaterThan,
				[Filter.ComparisonMap[Filter.Comparison.GreaterEqual]] = Expression.GreaterThanOrEqual,
				[Filter.ComparisonMap[Filter.Comparison.Less]] = Expression.LessThan,
				[Filter.ComparisonMap[Filter.Comparison.LessEqual]] = Expression.LessThanOrEqual,
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

		private readonly IResultsRepository _resultsRepository;

		public ResultService(IResultsRepository resultsRepository)
		{
			_resultsRepository = resultsRepository ?? throw new ArgumentNullException(nameof(resultsRepository));
		}

		#region IResultsService Members

		public Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			if (resultDto == null)
			{
				throw new ArgumentNullException(nameof(resultDto));
			}

			resultDto.TimeStamp = DateTime.UtcNow;
			return _resultsRepository.CreateAsync(resultDto.ToModel(), cancellationToken);
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

		private static LambdaExpression GetExpression<T>(Expression<Func<ResultModel, T>> baseExp)
		{
			// Dirty hack to prevent boxing of values
			return baseExp;
		}

		private static void AddFilter(ICollection<Expression<Func<ResultModel, bool>>> accumulator,
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

			accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(middlePart, parameters));
		}


		private static Expression<Func<ResultModel, object>> GetOrderBy(QueryRequest queryRequest)
		{
			if (queryRequest.OrderBy == null)
			{
				return null;
			}

			var orderBy = queryRequest.OrderBy.ToLowerInvariant();

			if (ModelExpressions.TryGetValue(orderBy, out var strExpression))
			{
				return Expression.Lambda<Func<ResultModel, object>>(
					Expression.Convert(strExpression.Body, typeof(object)), strExpression.Parameters);
			}

			throw new ArgumentException(
				$"OrderBy is not a valid order field. Can be: {string.Join(',', ModelExpressions.Keys)}");
		}

		private static IEnumerable<Expression<Func<ResultModel, bool>>> GetQueryFilters(QueryRequest queryRequest)
		{
			if (queryRequest.Filters == null)
			{
				return null;
			}

			var returnList = new List<Expression<Func<ResultModel, bool>>>();

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