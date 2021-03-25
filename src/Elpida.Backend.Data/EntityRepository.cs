using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
	public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		protected DbSet<TEntity> Collection { get; }
		protected DbContext Context { get; }

		public EntityRepository(DbContext context, DbSet<TEntity> collection)
		{
			Collection = collection;
			Context = context;
		}

		public Task<TEntity> GetSingleAsync(long id, CancellationToken cancellationToken = default)
		{
			return ProcessGetSingle(Collection.AsQueryable())
				.FirstAsync(e => e.Id == id, cancellationToken);
		}

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
		
		public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filters, CancellationToken cancellationToken = default)
		{
			return ProcessGetSingle(Collection.AsQueryable())
				.FirstOrDefaultAsync(filters, cancellationToken);
		}
		
		public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			var addedEntity = await Collection.AddAsync(entity, cancellationToken);
			return addedEntity.Entity;
		}

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

		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return Context.SaveChangesAsync(cancellationToken);
		}
	}
}