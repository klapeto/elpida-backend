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
	///     Details of a criteria for a search.
	/// </summary>
	public class QueryInstance
	{
		/// <summary>
		///     The name of the field this criteria applies.
		/// </summary>
		/// <example>benchmarkName</example>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		///     The value this criteria.
		/// </summary>
		/// <example>Allocate Memory</example>
		public object? Value { get; set; }

		/// <summary>
		///     The comparison to use on the field and the value.
		/// </summary>
		/// <example>eq</example>
		public string? Comp { get; set; } = string.Empty;
	}
}