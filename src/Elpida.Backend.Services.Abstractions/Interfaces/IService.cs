using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface IService<TDto>
    {
        Task<TDto> GetSingleAsync(long id, CancellationToken cancellationToken = default);
        Task<PagedResult<TDto>> GetPagedAsync(QueryRequest queryRequest, CancellationToken cancellationToken = default);
        Task<TDto> GetOrAddAsync(TDto dto, CancellationToken cancellationToken = default);
        IEnumerable<FilterExpression> GetFilters<T, TR>(Expression<Func<T, TR>> baseExpression);
    }
}