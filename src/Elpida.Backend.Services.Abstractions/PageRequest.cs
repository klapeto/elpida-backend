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
	///     A request for paged collection of objects.
	/// </summary>
	public class PageRequest
	{
		/// <summary>
		///     The maximum count that a page can have.
		/// </summary>
		public const int MaxCount = 500;

		/// <summary>
		///     The number of objects to skip.
		/// </summary>
		/// <example>0</example>
		public int Next { get; set; }

		/// <summary>
		///     The number of objects the page should have.
		/// </summary>
		/// <example>10</example>
		public int Count { get; set; } = 10;

		/// <summary>
		///     The total count of objects. This value is returned from
		///     the server and is ignored when set.
		/// </summary>
		/// <remarks>
		///     When requesting a page with this value set to 0, then server will
		///     set this to the total count of the objects with the returning <see cref="PageRequest" />
		///     or the <see cref="PagedResult{T}.TotalCount" /> of <see cref="PagedResult{T}" />
		///     (depending of which is returned).
		/// </remarks>
		/// <example>0</example>
		public long TotalCount { get; set; }
	}
}