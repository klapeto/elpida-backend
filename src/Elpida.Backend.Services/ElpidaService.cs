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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Models.Elpida;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Elpida;

namespace Elpida.Backend.Services
{
    public class ElpidaService : Service<ElpidaDto, ElpidaModel, IElpidaRepository>, IElpidaService
    {
        public ElpidaService(IElpidaRepository elpidaRepository, ILockFactory lockFactory)
            : base(elpidaRepository, lockFactory)
        {
        }
        
        private static IEnumerable<FilterExpression> ElpidaExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("compilerName", model => model.CompilerName),
            CreateFilter("compilerVersion", model => model.CompilerVersion),
            CreateFilter("buildVersion", model => model.VersionBuild),
            CreateFilter("revisionVersion", model => model.VersionRevision),
            CreateFilter("minorVersion", model => model.VersionMinor),
            CreateFilter("majorVersion", model => model.VersionMajor),
        };


        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return ElpidaExpressions;
        }

        protected override ElpidaDto ToDto(ElpidaModel model)
        {
            return model.ToDto();
        }

        protected override Task<ElpidaModel> ProcessDtoAndCreateModelAsync(ElpidaDto dto,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new ElpidaModel
            {
                Id = dto.Id,
                CompilerName = dto.Compiler.Name,
                CompilerVersion = dto.Compiler.Version,
                VersionMajor = dto.Version.Major,
                VersionMinor = dto.Version.Minor,
                VersionRevision = dto.Version.Revision,
                VersionBuild = dto.Version.Build
            });
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