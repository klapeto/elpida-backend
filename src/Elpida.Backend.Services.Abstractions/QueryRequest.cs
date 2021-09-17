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
	///     Details for a query.
	/// </summary>
	public sealed class QueryRequest
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="QueryRequest" /> class.
		/// </summary>
		/// <param name="pageRequest">The page to request.</param>
		/// <param name="filters">The query filters.</param>
		/// <param name="orderBy">The field that the results should be sorted.</param>
		/// <param name="descending">Whether the sorting should be descending.</param>
		public QueryRequest(
			PageRequest pageRequest,
			FilterInstance[]? filters,
			string? orderBy,
			bool descending
		)
		{
			Descending = descending;
			OrderBy = orderBy;
			PageRequest = pageRequest;
			Filters = filters;
		}

		/// <summary>
		///     Whether the elements returned should be ordered descending.
		/// </summary>
		public bool Descending { get; }

		/// <summary>
		///     The data field to be used to order the search results.
		/// </summary>
		/// <example>benchmarkName</example>
		public string? OrderBy { get; }

		/// <summary>
		///     The page of the search results to return.
		/// </summary>
		[Required]
		public PageRequest PageRequest { get; }

		/// <summary>
		///     The list of data filters to apply to the search.
		/// </summary>
		public FilterInstance[]? Filters { get; }
	}
}