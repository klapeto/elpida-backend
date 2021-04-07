using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface IElpidaService
    {
        Task<long> GetOrAddElpidaAsync(ElpidaDto elpidaDto, CancellationToken cancellationToken = default);
        Task<ElpidaDto> GetSingleAsync(long elpidaId, CancellationToken cancellationToken = default);
    }
}