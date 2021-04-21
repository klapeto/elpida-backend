using System.Threading;
using System.Threading.Tasks;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface IServiceWithPreviews<TDto, TPreviewDto> : IService<TDto>
    {
        Task<PagedResult<TPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default);
    }
}