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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions
{
	public interface IResultsRepository
	{
		Task<ResultModel> GetSingleAsync(string id, CancellationToken cancellationToken = default);

		Task<string> CreateAsync(ResultModel resultModel, CancellationToken cancellationToken = default);

		Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default);

		Task<PagedQueryResult<ResultPreviewModel>> GetAsync<TOrderKey>(
			int from, 
			int count, 
			bool descending, 
			Expression<Func<ResultModel, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<ResultModel, bool>>> filters,
			bool calculateTotalCount,
			CancellationToken cancellationToken = default);

		Task DeleteAllAsync(CancellationToken cancellationToken = default);
	}
}