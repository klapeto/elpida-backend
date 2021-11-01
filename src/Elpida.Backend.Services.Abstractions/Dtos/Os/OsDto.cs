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

namespace Elpida.Backend.Services.Abstractions.Dtos.Os
{
	/// <summary>
	///     Details of an Operating System.
	/// </summary>
	public sealed class OsDto : FoundationDto
	{
		/// <summary>
		///     The category of the Operating System.
		/// </summary>
		/// <example>GNU/Linux</example>
		[Required]
		[MaxLength(50)]
		public string Category { get; init; }

		/// <summary>
		///     The name of the Operating System.
		/// </summary>
		/// <example>Ubuntu</example>
		[Required]
		[MaxLength(100)]
		public string Name { get; init; }

		/// <summary>
		///     The version of the Operating System.
		/// </summary>
		/// <example>21.04</example>
		[Required]
		[MaxLength(50)]
		public string Version { get; init; }
	}
}