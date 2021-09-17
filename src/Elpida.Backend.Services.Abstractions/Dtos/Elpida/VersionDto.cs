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

using System.ComponentModel.DataAnnotations;

namespace Elpida.Backend.Services.Abstractions.Dtos.Elpida
{
	/// <summary>
	///     Details of a version.
	/// </summary>
	public sealed class VersionDto
	{
		public VersionDto(int major, int minor, int revision, int build)
		{
			Major = major;
			Minor = minor;
			Revision = revision;
			Build = build;
		}

		[Required]
		[Range(0, int.MaxValue)]
		public int Major { get; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Minor { get; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Revision { get; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Build { get; }
	}
}