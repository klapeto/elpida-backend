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
	///     Details of a criteria for a search.
	/// </summary>
	public sealed class FilterInstance
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="FilterInstance" /> class.
		/// </summary>
		/// <param name="name">The name of the field to filter.</param>
		/// <param name="value">The value of the filter.</param>
		/// <param name="comparison">The comparison that the filter should do.</param>
		public FilterInstance(string name, object value, string comparison)
		{
			Name = name;
			Value = value;
			Comparison = comparison;
		}

		/// <summary>
		///     The name of the field this criteria applies.
		/// </summary>
		/// <example>benchmarkName</example>
		public string Name { get; }

		/// <summary>
		///     The value this criteria.
		/// </summary>
		/// <example>Allocate Memory</example>
		public object Value { get; }

		/// <summary>
		///     The comparison to use on the field and the value.
		/// </summary>
		/// <example>equal</example>
		public string Comparison { get; }
	}
}