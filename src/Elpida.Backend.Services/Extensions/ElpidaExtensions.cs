/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
                VersionBuild = elpidaDto.Version.Build
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