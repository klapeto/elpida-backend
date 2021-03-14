/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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

namespace Elpida.Backend.Data.Abstractions.Interfaces
{
	public interface IRepository<T> where T: IEntity
	{
		Task<T> GetSingleAsync(string id, CancellationToken cancellationToken = default);

		Task<string> CreateAsync(T model, CancellationToken cancellationToken = default);

		Task UpdateAsync(T model, CancellationToken cancellationToken = default);

		Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default);

		Task<PagedQueryResult<T>> GetPagedAsync<TOrderKey>(
			int from, 
			int count, 
			bool descending, 
			Expression<Func<T, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<T, bool>>> filters,
			bool calculateTotalCount,
			CancellationToken cancellationToken = default);
		
		Task<List<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> filters,
			CancellationToken cancellationToken = default);

		Task DeleteAllAsync(CancellationToken cancellationToken = default);
	}
}