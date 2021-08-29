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

namespace Elpida.Backend.Services.Abstractions.Dtos.Elpida
{
	/// <summary>
	///     Details of a compiler.
	/// </summary>
	public sealed class CompilerDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CompilerDto" /> class.
		/// </summary>
		/// <param name="name">The name of the compiler.</param>
		/// <param name="version">The version of the compiler.</param>
		public CompilerDto(string name, string version)
		{
			Name = name;
			Version = version;
		}

		/// <summary>
		///     The name of the compiler.
		/// </summary>
		/// <example>GNU</example>
		public string Name { get; }

		/// <summary>
		///     The version of the compiler.
		/// </summary>
		/// <example>10.0</example>
		public string Version { get; }
	}
}