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
    public abstract class EntityRepositoryWithPreviews<TEntity, TPreviewEntity>
        : EntityRepository<TEntity>, IRepositoryWithPreviews<TEntity, TPreviewEntity> where TEntity : Entity
    {
        protected EntityRepositoryWithPreviews(DbContext context, DbSet<TEntity> collection) : base(context, collection)
        {
        }

        public async Task<PagedQueryResult<TPreviewEntity>> GetPagedPreviewsAsync<TOrderKey>(
            int from,
            int count,
            bool descending = false,
            bool calculateTotalCount = false,
            Expression<Func<TEntity, TOrderKey>>? orderBy = null,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters = null,
            CancellationToken cancellationToken = default)
        {
            var query = ProcessGetMultiplePaged(Collection.AsQueryable());
            if (from < 0) throw new ArgumentException("'from' must be positive or 0", nameof(@from));

            if (count <= 0) throw new ArgumentException("'count' must be positive", nameof(count));

            var result = query.AsNoTracking();

            if (filters != null) result = filters.Aggregate(result, (current, filter) => current.Where(filter));

            if (orderBy != null) result = @descending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);

            var totalCount = calculateTotalCount ? await result.CountAsync(cancellationToken) : 0;

            var results = await result
                .Skip(from)
                .Take(count)
                .Select(GetPreviewConstructionExpresion())
                .ToListAsync(cancellationToken);

            return new PagedQueryResult<TPreviewEntity>(totalCount, results);
        }

        protected abstract Expression<Func<TEntity, TPreviewEntity>> GetPreviewConstructionExpresion();
    }
}