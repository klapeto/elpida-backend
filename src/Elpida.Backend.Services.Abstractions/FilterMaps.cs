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

namespace Elpida.Backend.Services.Abstractions
{
	/// <summary>
	///     Provides maps for filters.
	/// </summary>
	public static class FilterMaps
	{
		/// <summary>
		///     Maps the <see cref="FilterComparison" /> to string values used by API callers.
		/// </summary>
		public static IReadOnlyDictionary<FilterComparison, string> ComparisonMap { get; } =
			new Dictionary<FilterComparison, string>
			{
				[FilterComparison.Equal] = "eq",
				[FilterComparison.NotEqual] = "neq",
				[FilterComparison.Contains] = "c",
				[FilterComparison.NotContain] = "nc",
				[FilterComparison.GreaterEqual] = "ge",
				[FilterComparison.Greater] = "g",
				[FilterComparison.LessEqual] = "le",
				[FilterComparison.Less] = "l",
			};

		/// <summary>
		///     A set of the allowed comparison string values (used by the API callers) for Numeric values.
		/// </summary>
		public static IReadOnlySet<string> NumberComparisons { get; } = new HashSet<string>
		{
			ComparisonMap[FilterComparison.Equal],
			ComparisonMap[FilterComparison.GreaterEqual],
			ComparisonMap[FilterComparison.Greater],
			ComparisonMap[FilterComparison.LessEqual],
			ComparisonMap[FilterComparison.Less],
		};

		/// <summary>
		///     A set of the allowed comparison string values (used by the API callers) for String values.
		/// </summary>
		public static IReadOnlySet<string> StringComparisons { get; } = new HashSet<string>
		{
			ComparisonMap[FilterComparison.Equal],
			ComparisonMap[FilterComparison.NotEqual],
			ComparisonMap[FilterComparison.Contains],
			ComparisonMap[FilterComparison.NotContain],
		};
	}
}