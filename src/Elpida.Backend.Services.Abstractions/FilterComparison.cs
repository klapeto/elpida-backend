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

namespace Elpida.Backend.Services.Abstractions
{
	/// <summary>
	///     A filter comparison type.
	/// </summary>
	public enum FilterComparison
	{
		/// <summary>
		///     The value must be equal.
		/// </summary>
		Equal,

		/// <summary>
		///     The value must not be equal.
		/// </summary>
		NotEqual,

		/// <summary>
		///     The value must contain.
		/// </summary>
		Contains,

		/// <summary>
		///     The value must not contain.
		/// </summary>
		NotContain,

		/// <summary>
		///     The value must be greater or equal.
		/// </summary>
		GreaterEqual,

		/// <summary>
		///     The value must be greater.
		/// </summary>
		Greater,

		/// <summary>
		///     The value must be less or equal.
		/// </summary>
		LessEqual,

		/// <summary>
		///     The value must be less.
		/// </summary>
		Less,
	}
}