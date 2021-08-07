/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2020 Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

namespace Elpida.Backend.Services.Abstractions
{
	/// <summary>
	///     Details for a query.
	/// </summary>
	public class QueryRequest
	{
		/// <summary>
		///     Whether the elements returned should be ordered descending.
		/// </summary>
		public bool Descending { get; set; }

		/// <summary>
		///     The data field to be used to order the search results.
		/// </summary>
		/// <example>benchmarkName</example>
		public string OrderBy { get; set; } = string.Empty;

		/// <summary>
		///     The page of the search results to return.
		/// </summary>
		public PageRequest PageRequest { get; set; } = new ();

		/// <summary>
		///     The list of data filters to apply to the search.
		/// </summary>
		public QueryInstance[]? Filters { get; set; }
	}
}