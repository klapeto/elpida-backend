using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface IOsService
    {
        Task<long> GetOrAddOsAsync(OsDto osDto, CancellationToken cancellationToken = default);
        Task<OsDto> GetSingleAsync(long osId, CancellationToken cancellationToken = default);
    }
}