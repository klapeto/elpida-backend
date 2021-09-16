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

namespace Elpida.Backend.Services.Abstractions.Dtos.Os
{
	/// <summary>
	///     Details of an Operating System.
	/// </summary>
	public sealed class OsDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="OsDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Operating System.</param>
		/// <param name="category">The category of the Operating System.</param>
		/// <param name="name">The name of the Operating System.</param>
		/// <param name="version">The version of the Operating System.</param>
		public OsDto(long id, string category, string name, string version)
			: base(id)
		{
			Category = category;
			Name = name;
			Version = version;
		}

		/// <summary>
		///     The category of the Operating System.
		/// </summary>
		/// <example>GNU/Linux</example>
		public string Category { get; }

		/// <summary>
		///     The name of the Operating System.
		/// </summary>
		/// <example>Ubuntu</example>
		public string Name { get; }

		/// <summary>
		///     The version of the Operating System.
		/// </summary>
		/// <example>21.04</example>
		public string Version { get; }
	}
}