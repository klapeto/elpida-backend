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

using System.Collections.Generic;
using System.Linq;

namespace Elpida.Backend.Services.Abstractions
{
	/// <summary>
	///     A page of results.
	/// </summary>
	/// <typeparam name="T">The underlying type of the items of the page.</typeparam>
	public sealed class PagedResult<T>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="PagedResult{T}" /> class.
		/// </summary>
		/// <param name="items">The collection of the items.</param>
		/// <param name="pageRequest">The previous page.</param>
		public PagedResult(IEnumerable<T> items, PageRequest pageRequest)
		{
			Items = items.ToArray();
			Count = Items.Length;

			TotalCount = pageRequest.TotalCount;

			NextPage = Items.Length == pageRequest.Count
				? new PageRequest(pageRequest.Next + Items.Length, pageRequest.Count, pageRequest.TotalCount)
				: null;
		}

		/// <summary>
		///     The actual list of the items.
		/// </summary>
		public T[] Items { get; }

		/// <summary>
		///     The count of the items this page has.
		/// </summary>
		public int Count { get; }

		/// <summary>
		///     The total count of items.
		/// </summary>
		public long TotalCount { get; }

		/// <summary>
		///     The next page to request the next items.
		/// </summary>
		public PageRequest? NextPage { get; }
	}
}