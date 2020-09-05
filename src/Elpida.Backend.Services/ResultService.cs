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
		private readonly IResultsRepository _resultsRepository;

		public ResultService(IResultsRepository resultsRepository)
		{
			_resultsRepository = resultsRepository ?? throw new ArgumentNullException(nameof(resultsRepository));
		}

		#region IResultsService Members

		public Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			if (resultDto == null) throw new ArgumentNullException(nameof(resultDto));
			resultDto.TimeStamp = DateTime.UtcNow;
			return _resultsRepository.CreateAsync(resultDto.ToModel(), cancellationToken);
		}

		public async Task<ResultDto> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be empty", nameof(id));

			var model = await _resultsRepository.GetSingleAsync(id, cancellationToken);
			if (model == null) throw new NotFoundException(id);

			return model.ToDto();
		}

		public async Task<PagedResult<ResultPreviewDto>> GetPagedAsync(QueryRequest queryRequest,
			CancellationToken cancellationToken)
		{
			if (queryRequest == null) throw new ArgumentNullException(nameof(queryRequest));

			var result = await _resultsRepository.GetAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				queryRequest.Descending,
				m => m.TimeStamp,
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

		private static void AddFilter(List<Expression<Func<ResultModel, bool>>> accumulator,
			QueryInstance<string> instance, Expression<Func<ResultModel, string>> fieldPart)
		{
			if (instance.Comp == "c" || instance.Comp == null)
				accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(
					Expression.Call(
						fieldPart.Body,
						typeof(string).GetMethod(nameof(string.Contains),new []{typeof(string)}),
						Expression.Constant(instance.Value)
					),
					fieldPart.Parameters)
				);
			else if (instance.Comp == "eq")
				accumulator.Add(Expression.Lambda<Func<ResultModel, bool>>(Expression.Equal(fieldPart.Body,
						Expression.Constant(instance.Value)),
					fieldPart.Parameters));
			else
				throw new ArgumentException(
					"String filter needs either 'c'/null for checking if the field contains or 'eq' for equality");
		}


		private static IEnumerable<Expression<Func<ResultModel, bool>>> GetQueryFilters(QueryRequest queryRequest)
		{
			var returnList = new List<Expression<Func<ResultModel, bool>>>();
			AddFilter(returnList, queryRequest.BenchmarkName, model => model.Result.Name);
			// if (queryRequest.BenchmarkName != null)
			// {
			// 	if(queryRequest.BenchmarkName.Comp == "c") 
			// 		returnList.Add(m => m.Result.Name.Contains(queryRequest.BenchmarkName.Value));
			// 	else
			// 		returnList.Add(m => m.Result.Name== queryRequest.BenchmarkName.Value);
			// }


			return returnList;
		}
	}
}