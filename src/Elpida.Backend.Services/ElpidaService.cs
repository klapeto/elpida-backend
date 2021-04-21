using System;
using System.Linq.Expressions;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;

namespace Elpida.Backend.Services
{
    public class ElpidaService : Service<ElpidaDto, ElpidaModel, IElpidaRepository>, IElpidaService
    {
        public ElpidaService(IElpidaRepository elpidaRepository)
            : base(elpidaRepository)
        {
        }
        
        protected override ElpidaDto ToDto(ElpidaModel model)
        {
            return model.ToDto();
        }

        protected override ElpidaModel ToModel(ElpidaDto dto)
        {
            return dto.ToModel();
        }

        protected override Expression<Func<ElpidaModel, bool>> GetCreationBypassCheckExpression(ElpidaDto dto)
        {
            return e =>
                e.VersionMajor == dto.Version.Major
                && e.VersionMinor == dto.Version.Minor
                && e.VersionRevision == dto.Version.Revision
                && e.VersionBuild == dto.Version.Build
                && e.CompilerName == dto.Compiler.Name
                && e.CompilerVersion == dto.Compiler.Version;
        }
    }
}