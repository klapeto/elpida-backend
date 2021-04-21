using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data.Abstractions.Interfaces
{
    public interface IRepositoryWithPreviews<TEntity, TPreviewEntity>
        : IRepository<TEntity> where TEntity : Entity
    {
        Task<PagedQueryResult<TPreviewEntity>> GetPagedPreviewsAsync<TOrderKey>(
            int from,
            int count,
            bool descending = false,
            bool calculateTotalCount = false,
            Expression<Func<TEntity, TOrderKey>>? orderBy = null,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters = null,
            CancellationToken cancellationToken = default);
    }
}