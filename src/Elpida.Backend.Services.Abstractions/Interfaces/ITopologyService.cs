using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface ITopologyService
    {
        Task<long> GetOrAddTopologyAsync(long cpuId, TopologyDto topologyDto, CancellationToken cancellationToken = default);
        Task<TopologyDto> GetSingleAsync(long topologyId, CancellationToken cancellationToken = default);
    }
}