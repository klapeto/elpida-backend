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
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private static readonly Dictionary<string, LambdaExpression> Expressions =
			new Dictionary<string, LambdaExpression>
			{
				["cpuCores".ToLowerInvariant()] = GetExpression(model => model.System.Topology.TotalPhysicalCores),
				["cpuLogicalCores".ToLowerInvariant()] =
					GetExpression(model => model.System.Topology.TotalLogicalCores),
				["cpuCores".ToLowerInvariant()] = GetExpression(model => model.System.Topology.TotalPhysicalCores),
				["cpuLogicalCores".ToLowerInvariant()] =
					GetExpression(model => model.System.Topology.TotalLogicalCores),
				["cpuFrequency".ToLowerInvariant()] = GetExpression(model => model.System.Cpu.Frequency),
				["memorySize".ToLowerInvariant()] = GetExpression(model => model.System.Memory.TotalSize),
				["timestamp".ToLowerInvariant()] = GetExpression(model => model.TimeStamp),
				["name".ToLowerInvariant()] = GetExpression(model => model.Result.Name),
				["cpuBrand".ToLowerInvariant()] = GetExpression(model => model.System.Cpu.Brand),
				["cpuVendor".ToLowerInvariant()] = GetExpression(model => model.System.Cpu.Vendor),
				["osCategory".ToLowerInvariant()] = GetExpression(model => model.System.Os.Category),
				["osName".ToLowerInvariant()] = GetExpression(model => model.System.Os.Name),
				["osVersion".ToLowerInvariant()] = GetExpression(model => model.System.Os.Version)
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

			if (queryRequest.Filters != null)
			{
				foreach (var queryInstance in queryRequest.Filters.Where(f => f.Name == "startTime"))
				{
					queryInstance.Comp = "ge";
				}

				foreach (var queryInstance in queryRequest.Filters.Where(f => f.Name == "endTime"))
				{
					queryInstance.Comp = "le";
				}
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

		private static IEnumerable<string> GetAllFilterKeys()
		{
			return Expressions.Keys;
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
				switch (instance.Comp)
				{
					case "c":
					case null:
						middlePart = Expression.Call(
							left,
							typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)}),
							right);
						break;
					case "eq":
						middlePart = Expression.Equal(left, right);
						break;
					default:
						throw new ArgumentException(
							"String filter needs either 'c'/null for checking if the field contains or 'eq' for equality");
				}
			}
			else
			{
				middlePart = instance.Comp switch
				{
					"g" => Expression.GreaterThan(left, right),
					"ge" => Expression.GreaterThanOrEqual(left, right),
					"l" => Expression.LessThan(left, right),
					"le" => Expression.LessThanOrEqual(left, right),
					"eq" => Expression.Equal(left, right),
					_ => throw new ArgumentException(
						"Numeric value filter comparison types can be :[g,ge,l,le,eq] (Greater, Greater/Equal, Less, Less/Equal, Equal)")
				};
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

			if (Expressions.TryGetValue(orderBy, out var strExpression))
			{
				return Expression.Lambda<Func<ResultModel, object>>(
					Expression.Convert(strExpression.Body, typeof(object)), strExpression.Parameters);
			}

			throw new ArgumentException(
				$"OrderBy is not a valid order field. Can be: {string.Join(',', GetAllFilterKeys())}");
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
				if (Expressions.TryGetValue(filter.Name.ToLowerInvariant(), out var expression))
				{
					AddFilter(returnList, filter, expression);
				}
			}

			return returnList;
		}
	}
}