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
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public EntityRepository(DbContext context, DbSet<TEntity> collection)
        {
            Collection = collection;
            Context = context;
        }

        protected DbSet<TEntity> Collection { get; }
        protected DbContext Context { get; }

        protected virtual IQueryable<TEntity> ProcessGetSingle(IQueryable<TEntity> queryable)
        {
            return queryable;
        }

        protected virtual IQueryable<TEntity> ProcessGetMultiplePaged(IQueryable<TEntity> queryable)
        {
            return queryable;
        }

        #region IRepository<TEntity> Members

        public async Task<TEntity?> GetSingleAsync(long id, CancellationToken cancellationToken = default)
        {
            return await ProcessGetSingle(Collection.AsQueryable())
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> filters,
            CancellationToken cancellationToken = default)
        {
            return await ProcessGetSingle(Collection.AsQueryable())
                .FirstOrDefaultAsync(filters, cancellationToken);
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var addedEntity = await Collection.AddAsync(entity, cancellationToken);
            return addedEntity.Entity;
        }

        public Task<PagedQueryResult<TEntity>> GetMultiplePagedAsync<TOrderKey>(
            int from,
            int count,
            bool descending,
            bool calculateTotalCount,
            Expression<Func<TEntity, TOrderKey>>? orderBy,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionAsync(from, count, m => m, descending, calculateTotalCount, orderBy, filters,
                cancellationToken);
        }

        public async Task<PagedQueryResult<TReturnEntity>> GetPagedProjectionAsync<TOrderKey, TReturnEntity>(
            int from,
            int count,
            Expression<Func<TEntity, TReturnEntity>> constructionExpression,
            bool descending = false,
            bool calculateTotalCount = false,
            Expression<Func<TEntity, TOrderKey>>? orderBy = null,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters = null,
            CancellationToken cancellationToken = default)
        {
            var query = ProcessGetMultiplePaged(Collection.AsQueryable());
            if (from < 0) throw new ArgumentException("'from' must be positive or 0", nameof(from));

            if (count <= 0) throw new ArgumentException("'count' must be positive", nameof(count));

            var result = query.AsNoTracking();

            if (filters != null) result = filters.Aggregate(result, (current, filter) => current.Where(filter));

            if (orderBy != null) result = descending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);

            var totalCount = calculateTotalCount ? await result.CountAsync(cancellationToken) : 0;

            var results = await result
                .Skip(from)
                .Take(count)
                .Select(constructionExpression)
                .ToListAsync(cancellationToken);

            return new PagedQueryResult<TReturnEntity>(totalCount, results);
        }

        public Task<List<TEntity>> GetMultipleAsync(IEnumerable<Expression<Func<TEntity, bool>>> filters,
            CancellationToken cancellationToken = default)
        {
            var result = ProcessGetMultiplePaged(Collection.AsQueryable())
                .AsNoTracking();
            result = filters.Aggregate(result, (current, filter) => current.Where(filter));

            return result.ToListAsync(cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}