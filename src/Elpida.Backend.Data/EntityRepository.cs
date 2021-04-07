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
		
		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return Context.SaveChangesAsync(cancellationToken);
		}
		
#if false
		public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			Collection.Update(entity);
			return Task.CompletedTask;
		}

		public Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default)
		{
			return Collection.LongCountAsync(cancellationToken);
		}

		public async Task<PagedQueryResult<TEntity>> GetMultiplePagedAsync<TOrderKey>(
			int from,
			int count,
			bool descending,
			Expression<Func<TEntity, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<TEntity, bool>>> filters,
			bool calculateTotalCount,
			CancellationToken cancellationToken = default)
		{
			if (from < 0)
			{
				throw new ArgumentException("'from' must be positive or 0", nameof(from));
			}

			if (count <= 0)
			{
				throw new ArgumentException("'count' must be positive", nameof(count));
			}

			var query = Collection.AsQueryable();

			if (filters != null)
			{
				query = filters.Aggregate(query, (current, filter) => current.Where(filter));
			}

			var totalCount = calculateTotalCount ? await query.CountAsync(cancellationToken) : 0;

			if (orderBy != null)
			{
				query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
			}

			query = ProcessGetMultiplePaged(query);

			var results = await query
				.Skip(from)
				.Take(count)
				.ToListAsync(cancellationToken);

			return new PagedQueryResult<TEntity>(totalCount, results);
		}

		public Task<List<TEntity>> GetMultipleAsync(IEnumerable<Expression<Func<TEntity, bool>>> filters,
			CancellationToken cancellationToken = default)
		{
			var query = Collection.AsQueryable();

			if (filters != null)
			{
				query = filters.Aggregate(query, (current, filter) => current.Where(filter));
			}

			query = ProcessGetMultiple(query);

			return query.ToListAsync(cancellationToken);
		}

		public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			Collection.Remove(entity);
			return Task.CompletedTask;
		}
		
#endif
		#endregion

		protected virtual IQueryable<TEntity> ProcessGetSingle(IQueryable<TEntity> queryable)
		{
			return queryable;
		}

		protected virtual IQueryable<TEntity> ProcessGetMultiplePaged(IQueryable<TEntity> queryable)
		{
			return queryable;
		}

		protected virtual IQueryable<TEntity> ProcessGetMultiple(IQueryable<TEntity> queryable)
		{
			return queryable;
		}
	}
}