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

using Elpida.Backend.Data.Abstractions.Models.ElpidaVersion;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;

namespace Elpida.Backend.Services.Extensions.Elpida
{
	public static class ElpidaVersionExtensions
	{
		public static ElpidaVersionDto ToDto(this ElpidaVersionModel elpidaVersionModel)
		{
			return new ()
			{
				Id = elpidaVersionModel.Id,
				Version = new VersionDto
				{
					Major = elpidaVersionModel.VersionMajor,
					Minor = elpidaVersionModel.VersionMinor,
					Revision = elpidaVersionModel.VersionRevision,
					Build = elpidaVersionModel.VersionBuild,
				},
				Compiler = new CompilerDto
				{
					Name = elpidaVersionModel.CompilerName,
					Version = elpidaVersionModel.CompilerVersion,
				},
			};
		}
	}
}