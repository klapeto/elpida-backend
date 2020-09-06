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
		private static readonly Dictionary<string, Expression<Func<ResultModel, object>>> OrderByExpressions =
			new Dictionary<string, Expression<Func<ResultModel, object>>>
			{
				[nameof(QueryRequest.Filters.Name).ToLowerInvariant()] = model => model.Result.Name,
				[nameof(QueryRequest.Filters.CpuBrand).ToLowerInvariant()] = model => model.System.Cpu.Brand,
				[nameof(QueryRequest.Filters.CpuVendor).ToLowerInvariant()] = model => model.System.Cpu.Vendor,
				[nameof(QueryRequest.Filters.CpuCores).ToLowerInvariant()] =
					model => model.System.Topology.TotalPhysicalCores,
				[nameof(QueryRequest.Filters.CpuLogicalCores).ToLowerInvariant()] =
					model => model.System.Topology.TotalLogicalCores,
				[nameof(QueryRequest.Filters.CpuFrequency).ToLowerInvariant()] = model => model.System.Cpu.Frequency,
				[nameof(QueryRequest.Filters.MemorySize).ToLowerInvariant()] = model => model.System.Memory.TotalSize,
				[nameof(QueryRequest.Filters.OsCategory).ToLowerInvariant()] = model => model.System.Os.Category,
				[nameof(QueryRequest.Filters.OsName).ToLowerInvariant()] = model => model.System.Os.Name,
				[nameof(QueryRequest.Filters.OsVersion).ToLowerInvariant()] = model => model.System.Os.Version,
				["timestamp"] = model => model.TimeStamp
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

			if (queryRequest.Filters?.StartTime != null)
			{
				queryRequest.Filters.StartTime.Comp = "ge";
			}

			if (queryRequest.Filters?.EndTime != null)
			{
				queryRequest.Filters.EndTime.Comp = "le";
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

		private static void AddFilter<T>(ICollection<Expression<Func<ResultModel, bool>>> accumulator,
			QueryInstance<T> instance, Expression<Func<ResultModel, T>> fieldPart)
		{
			if (instance == null)
			{
				return;
			}

			switch (instance.Comp)
			{
				case "g":
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.GreaterThan(fieldPart.Body,
							Expression.Constant(instance.Value)),
						fieldPart.Parameters));
					break;
				case "ge":
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.GreaterThanOrEqual(
							fieldPart.Body,
							Expression.Constant(instance.Value)),
						fieldPart.Parameters));
					break;
				case "l":
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.LessThan(fieldPart.Body,
							Expression.Constant(instance.Value)),
						fieldPart.Parameters));
					break;
				case "le":
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.LessThan(fieldPart.Body,
							Expression.Constant(instance.Value)),
						fieldPart.Parameters));
					break;
				case "eq":
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.Equal(fieldPart.Body,
							Expression.Constant(instance.Value)),
						fieldPart.Parameters));
					break;
				default:
					throw new ArgumentException(
						"Numeric value filter comparison types can be :[g,ge,l,le,eq] (Greater, Greater/Equal, Less, Less/Equal, Equal)");
			}
		}

		private static Expression<Func<ResultModel, object>> GetOrderBy(QueryRequest queryRequest)
		{
			if (queryRequest.OrderBy == null)
			{
				return null;
			}

			var orderBy = queryRequest.OrderBy.ToLowerInvariant();

			if (OrderByExpressions.TryGetValue(orderBy, out var expression))
			{
				return expression;
			}

			throw new ArgumentException(
				$"OrderBy is not a valid order field. Can be: {string.Join(',', OrderByExpressions.Keys)}");
		}

		private static void AddFilter(ICollection<Expression<Func<ResultModel, bool>>> accumulator,
			QueryInstance<string> instance, Expression<Func<ResultModel, string>> fieldPart)
		{
			if (instance == null)
			{
				return;
			}

			switch (instance.Comp)
			{
				case "c":
				case null:
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(
						Expression.Call(
							fieldPart.Body,
							typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)}),
							Expression.Constant(instance.Value)
						),
						fieldPart.Parameters)
					);
					break;
				case "eq":
					accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.Equal(fieldPart.Body,
							Expression.Constant(instance.Value)),
						fieldPart.Parameters));
					break;
				default:
					throw new ArgumentException(
						"String filter needs either 'c'/null for checking if the field contains or 'eq' for equality");
			}
		}

		private static IEnumerable<Expression<Func<ResultModel, bool>>> GetQueryFilters(QueryRequest queryRequest)
		{
			if (queryRequest.Filters == null)
			{
				return null;
			}

			var returnList = new List<Expression<Func<ResultModel, bool>>>();

			AddFilter(returnList, queryRequest.Filters.Name, model => model.Result.Name);

			AddFilter(returnList, queryRequest.Filters.OsCategory, model => model.System.Os.Category);
			AddFilter(returnList, queryRequest.Filters.OsName, model => model.System.Os.Name);
			AddFilter(returnList, queryRequest.Filters.OsVersion, model => model.System.Os.Version);

			AddFilter(returnList, queryRequest.Filters.MemorySize, model => model.System.Memory.TotalSize);

			AddFilter(returnList, queryRequest.Filters.EndTime, model => model.TimeStamp);
			AddFilter(returnList, queryRequest.Filters.StartTime, model => model.TimeStamp);

			AddFilter(returnList, queryRequest.Filters.CpuBrand, model => model.System.Cpu.Brand);
			AddFilter(returnList, queryRequest.Filters.CpuVendor, model => model.System.Cpu.Vendor);
			AddFilter(returnList, queryRequest.Filters.CpuCores, model => model.System.Topology.TotalPhysicalCores);
			AddFilter(returnList, queryRequest.Filters.CpuLogicalCores,
				model => model.System.Topology.TotalLogicalCores);
			AddFilter(returnList, queryRequest.Filters.CpuLogicalCores,
				model => model.System.Topology.TotalLogicalCores);
			AddFilter(returnList, queryRequest.Filters.CpuFrequency, model => model.System.Cpu.Frequency);

			return returnList;
		}
	}
}