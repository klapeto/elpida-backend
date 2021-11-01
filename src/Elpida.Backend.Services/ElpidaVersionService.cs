// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.ElpidaVersion;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Elpida;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
	public class ElpidaVersionService
		: Service<ElpidaVersionDto, ElpidaVersionDto, ElpidaVersionModel, IElpidaVersionRepository>,
			IElpidaVersionService
	{
		public ElpidaVersionService(IElpidaVersionRepository elpidaRepository)
			: base(elpidaRepository)
		{
		}

		private static IEnumerable<FilterExpression> ElpidaExpressions { get; } = new List<FilterExpression>
		{
			FiltersTransformer.CreateFilter<ElpidaVersionModel, string>("compilerName", model => model.CompilerName),
			FiltersTransformer.CreateFilter<ElpidaVersionModel, string>(
				"compilerVersion",
				model => model.CompilerVersion
			),
			FiltersTransformer.CreateFilter<ElpidaVersionModel, int>("buildVersion", model => model.VersionBuild),
			FiltersTransformer.CreateFilter<ElpidaVersionModel, int>("revisionVersion", model => model.VersionRevision),
			FiltersTransformer.CreateFilter<ElpidaVersionModel, int>("minorVersion", model => model.VersionMinor),
			FiltersTransformer.CreateFilter<ElpidaVersionModel, int>("majorVersion", model => model.VersionMajor),
		};

		public override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			return ElpidaExpressions;
		}

		protected override ElpidaVersionDto ToDto(ElpidaVersionModel versionModel)
		{
			return versionModel.ToDto();
		}

		protected override Expression<Func<ElpidaVersionModel, ElpidaVersionDto>> GetPreviewConstructionExpression()
		{
			return m => new ElpidaVersionDto
			{
				Id = m.Id,
				Version = new VersionDto
				{
					Major = m.VersionMajor,
					Minor = m.VersionMinor,
					Revision = m.VersionRevision,
					Build = m.VersionBuild,
				},
				Compiler = new CompilerDto
				{
					Name = m.CompilerName,
					Version = m.CompilerVersion,
				},
			};
		}

		protected override Task<ElpidaVersionModel> ProcessDtoAndCreateModelAsync(
			ElpidaVersionDto versionDto,
			CancellationToken cancellationToken
		)
		{
			return Task.FromResult(
				new ElpidaVersionModel
				{
					Id = versionDto.Id,
					CompilerName = versionDto.Compiler.Name,
					CompilerVersion = versionDto.Compiler.Version,
					VersionMajor = versionDto.Version.Major,
					VersionMinor = versionDto.Version.Minor,
					VersionRevision = versionDto.Version.Revision,
					VersionBuild = versionDto.Version.Build,
				}
			);
		}

		protected override Expression<Func<ElpidaVersionModel, bool>> GetCreationBypassCheckExpression(
			ElpidaVersionDto versionDto
		)
		{
			return e =>
				e.VersionMajor == versionDto.Version.Major
				&& e.VersionMinor == versionDto.Version.Minor
				&& e.VersionRevision == versionDto.Version.Revision
				&& e.VersionBuild == versionDto.Version.Build
				&& e.CompilerName == versionDto.Compiler.Name
				&& e.CompilerVersion == versionDto.Compiler.Version;
		}
	}
}