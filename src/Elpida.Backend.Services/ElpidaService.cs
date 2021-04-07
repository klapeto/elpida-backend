using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;

namespace Elpida.Backend.Services
{
    public class ElpidaService: IElpidaService
    {
        private readonly IElpidaRepository _elpidaRepository;

        public ElpidaService(IElpidaRepository elpidaRepository)
        {
            _elpidaRepository = elpidaRepository;
        }

        public async Task<long> GetOrAddElpidaAsync(ElpidaDto elpidaDto, CancellationToken cancellationToken = default)
        {
            var elpidaModel = await _elpidaRepository.GetSingleAsync(e =>
                e.VersionMajor == elpidaDto.Version.Major
                && e.VersionMinor == elpidaDto.Version.Minor
                && e.VersionRevision == elpidaDto.Version.Revision
                && e.VersionBuild == elpidaDto.Version.Build
                && e.CompilerName == elpidaDto.Compiler.Name
                && e.CompilerVersion == elpidaDto.Compiler.Version, 
                cancellationToken);

            if (elpidaModel != null) return elpidaModel.Id;

            elpidaModel = elpidaDto.ToModel();

            elpidaModel = await _elpidaRepository.CreateAsync(elpidaModel, cancellationToken);

            await _elpidaRepository.SaveChangesAsync(cancellationToken);

            return elpidaModel.Id;
        }

        public async Task<ElpidaDto> GetSingleAsync(long elpidaId, CancellationToken cancellationToken = default)
        {
            var elpidaModel = await _elpidaRepository.GetSingleAsync(elpidaId, cancellationToken);

            if (elpidaModel == null) throw new NotFoundException("Elpida was not found.", elpidaId);

            return elpidaModel.ToDto();
        }
    }
}