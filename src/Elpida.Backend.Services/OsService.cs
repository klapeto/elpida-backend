using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;

namespace Elpida.Backend.Services
{
    public class OsService : IOsService
    {
        private readonly IOsRepository _osRepository;

        public OsService(IOsRepository osRepository)
        {
            _osRepository = osRepository;
        }

        public async Task<long> GetOrAddOsAsync(OsDto osDto, CancellationToken cancellationToken)
        {
            var osModel = await _osRepository.GetSingleAsync(o =>
                o.Category == osDto.Category
                && o.Name == osDto.Name
                && o.Version == osDto.Version, cancellationToken);

            if (osModel != null) return osModel.Id;

            osDto.Id = 0;
            osModel = osDto.ToModel();

            osModel = await _osRepository.CreateAsync(osModel, cancellationToken);

            await _osRepository.SaveChangesAsync(cancellationToken);

            return osModel.Id;
        }

        public async Task<OsDto> GetSingleAsync(long osId, CancellationToken cancellationToken = default)
        {
            var osModel = await _osRepository.GetSingleAsync(osId, cancellationToken);

            if (osModel == null) throw new NotFoundException("Os was not found.", osId);

            return osModel.ToDto();
        }
    }
}