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

namespace Elpida.Backend.Services.Abstractions
{
	/// <summary>
	///     A request for paged collection of objects.
	/// </summary>
	public sealed class PageRequest
	{
		/// <summary>
		///     The maximum count that a page can have.
		/// </summary>
		public const int MaxCount = 500;

		/// <summary>
		///     Initializes a new instance of the <see cref="PageRequest" /> class.
		/// </summary>
		/// <param name="next">The items that should be skipped.</param>
		/// <param name="count">How many items should be returned.</param>
		public PageRequest(int next, int count)
		{
			Next = next;
			Count = count;
		}

		public PageRequest()
		{
		}

		/// <summary>
		///     The number of objects to skip.
		/// </summary>
		[Range(0, int.MaxValue)]
		public int Next { get; init; }

		/// <summary>
		///     The number of objects the page should have.
		/// </summary>
		/// <example>10</example>
		[Required]
		[Range(1, MaxCount)]
		public int Count { get; init; }
	}
}