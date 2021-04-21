/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data.Abstractions.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity?> GetSingleAsync(long id, CancellationToken cancellationToken = default);

        Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> filters,
            CancellationToken cancellationToken = default);

        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<PagedQueryResult<TEntity>> GetMultiplePagedAsync<TOrderKey>(
            int from,
            int count,
            bool descending = false,
            bool calculateTotalCount = false,
            Expression<Func<TEntity, TOrderKey>>? orderBy = null,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters = null,
            CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetMultipleAsync(IEnumerable<Expression<Func<TEntity, bool>>> filters,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}