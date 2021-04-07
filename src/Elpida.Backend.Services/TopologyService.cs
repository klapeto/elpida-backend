using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;
using Elpida.Backend.Services.Extensions.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
    public class TopologyService: ITopologyService
    {
        private readonly ICpuService _cpuService;
        private readonly ITopologyRepository _topologyRepository;

        public TopologyService(ITopologyRepository topologyRepository, ICpuService cpuService)
        {
            _topologyRepository = topologyRepository;
            _cpuService = cpuService;
        }

        public async Task<long> GetOrAddTopologyAsync(long cpuId, TopologyDto topologyDto, CancellationToken cancellationToken)
        {
            var cpu = await _cpuService.GetSingleAsync(cpuId, cancellationToken);
            
            var topologyModel = topologyDto.ToModel();

            topologyModel = await _topologyRepository.GetSingleAsync(t =>
                    t.CpuId == cpu.Id
                    && t.TopologyHash == topologyModel.TopologyHash,
                cancellationToken);

            if (topologyModel != null) return topologyModel.Id;
            
            topologyModel = topologyDto.ToModel();
            topologyModel.CpuId = cpuId;
            
            topologyModel = await _topologyRepository.CreateAsync(topologyModel, cancellationToken);

            await _topologyRepository.SaveChangesAsync(cancellationToken);

            return topologyModel.Id;
        }

        public async Task<TopologyDto> GetSingleAsync(long topologyId, CancellationToken cancellationToken = default)
        {
            var topologyModel = await _topologyRepository.GetSingleAsync(topologyId, cancellationToken);

            if (topologyModel == null) throw new NotFoundException("Topology was not found.", topologyId);

            return topologyModel.ToDto();
        }
    }
}