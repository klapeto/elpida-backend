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
	public static class FilterHelpers
	{
		public enum Comparison
		{
			Equal,
			NotEqual,
			Contains,
			NotContain,
			GreaterEqual,
			Greater,
			LessEqual,
			Less,
		}

		public static IReadOnlyDictionary<Comparison, string> ComparisonMap { get; } =
			new Dictionary<Comparison, string>
			{
				[Comparison.Equal] = "eq",
				[Comparison.NotEqual] = "neq",
				[Comparison.Contains] = "c",
				[Comparison.NotContain] = "nc",
				[Comparison.GreaterEqual] = "ge",
				[Comparison.Greater] = "g",
				[Comparison.LessEqual] = "le",
				[Comparison.Less] = "l",
			};

		public static HashSet<string> NumberComparisons { get; } = new ()
		{
			ComparisonMap[Comparison.Equal],
			ComparisonMap[Comparison.GreaterEqual],
			ComparisonMap[Comparison.Greater],
			ComparisonMap[Comparison.LessEqual],
			ComparisonMap[Comparison.Less],
		};

		public static HashSet<string> StringComparisons { get; } = new ()
		{
			ComparisonMap[Comparison.Equal],
			ComparisonMap[Comparison.NotEqual],
			ComparisonMap[Comparison.Contains],
			ComparisonMap[Comparison.NotContain],
		};
	}
}