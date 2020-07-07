using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Abstractions
{
	public interface IResultsService
	{
		Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken = default);

		Task<ResultDto> GetSingleAsync(string id, CancellationToken cancellationToken = default);

		Task<PagedResult<ResultPreviewDto>> GetPagedAsync(PageRequest pageRequest,
			CancellationToken cancellationToken = default);

		Task ClearResultsAsync(CancellationToken cancellationToken = default);
	}
}