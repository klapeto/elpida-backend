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
	///     Details of an Elpida version.
	/// </summary>
	public sealed class ElpidaVersionDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ElpidaVersionDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Elpida.</param>
		/// <param name="version">The Elpida version.</param>
		/// <param name="compiler">The compiler details that built Elpida.</param>
		public ElpidaVersionDto(long id, VersionDto version, CompilerDto compiler)
			: base(id)
		{
			Version = version;
			Compiler = compiler;
		}

		/// <summary>
		///     The Elpida version.
		/// </summary>
		[Required]
		public VersionDto Version { get; }

		/// <summary>
		///     The compiler details that built Elpida.
		/// </summary>
		[Required]
		public CompilerDto Compiler { get; }
	}
}