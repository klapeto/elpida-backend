using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions
{
    public static class ElpidaExtensions
    {
        public static ElpidaModel ToModel(this ElpidaDto elpidaDto)
        {
            return new ElpidaModel
            {
                Id = elpidaDto.Id,
                CompilerName = elpidaDto.Compiler.Name,
                CompilerVersion = elpidaDto.Compiler.Version,
                VersionMajor = elpidaDto.Version.Major,
                VersionMinor = elpidaDto.Version.Minor,
                VersionRevision = elpidaDto.Version.Revision,
                VersionBuild = elpidaDto.Version.Build,
            };
        }
        
        public static ElpidaDto ToDto(this ElpidaModel elpidaModel)
        {
            return new ElpidaDto
            {
                Id = elpidaModel.Id,
                Compiler = new CompilerDto
                {
                    Name = elpidaModel.CompilerName,
                    Version = elpidaModel.CompilerVersion
                },
                Version = new VersionDto
                {
                    Major = elpidaModel.VersionMajor,
                    Minor = elpidaModel.VersionMinor,
                    Revision = elpidaModel.VersionRevision,
                    Build = elpidaModel.VersionBuild
                }
            };
        }
    }
}