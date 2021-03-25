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
	public interface IRepository<TEntity> where TEntity : Entity
	{
		Task<TEntity> GetSingleAsync(long id, CancellationToken cancellationToken = default);

		Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filters,
			CancellationToken cancellationToken = default);

		Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

#if false
		Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

		Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default);

		Task<PagedQueryResult<TEntity>> GetMultiplePagedAsync<TOrderKey>(
			int from,
			int count,
			bool descending,
			Expression<Func<TEntity, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<TEntity, bool>>> filters,
			bool calculateTotalCount,
			CancellationToken cancellationToken = default);

		Task<List<TEntity>> GetMultipleAsync(IEnumerable<Expression<Func<TEntity, bool>>> filters,
			CancellationToken cancellationToken = default);

		Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
#endif

		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}