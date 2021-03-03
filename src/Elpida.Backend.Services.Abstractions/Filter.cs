/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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

using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions
{
	public static class Filter
	{
		#region Comparison enum

		public enum Comparison
		{
			Equal,
			Contains,
			GreaterEqual,
			Greater,
			LessEqual,
			Less
		}

		#endregion

		#region Type enum

		public enum Type
		{
			Timestamp,
			CpuCores,
			CpuLogicalCores,
			CpuFrequency,
			MemorySize,
			Name,
			CpuBrand,
			CpuVendor,
			OsCategory,
			OsName,
			OsVersion
		}

		#endregion

		public static IReadOnlyDictionary<Type, string> TypeMap { get; } = new Dictionary<Type, string>
		{
			[Type.Timestamp] = "timeStamp".ToLowerInvariant(),
			[Type.CpuCores] = "cpuCores".ToLowerInvariant(),
			[Type.CpuLogicalCores] = "cpuLogicalCores".ToLowerInvariant(),
			[Type.CpuFrequency] = "cpuFrequency".ToLowerInvariant(),
			[Type.MemorySize] = "memorySize".ToLowerInvariant(),
			[Type.Name] = "name".ToLowerInvariant(),
			[Type.CpuBrand] = "cpuBrand".ToLowerInvariant(),
			[Type.CpuVendor] = "cpuVendor".ToLowerInvariant(),
			[Type.OsCategory] = "osCategory".ToLowerInvariant(),
			[Type.OsName] = "osName".ToLowerInvariant(),
			[Type.OsVersion] = "osVersion".ToLowerInvariant()
		};

		public static IReadOnlyDictionary<Comparison, string> ComparisonMap { get; } =
			new Dictionary<Comparison, string>
			{
				[Comparison.Equal] = "eq",
				[Comparison.Contains] = "c",
				[Comparison.GreaterEqual] = "ge",
				[Comparison.Greater] = "g",
				[Comparison.LessEqual] = "le",
				[Comparison.Less] = "l"
			};

		public static HashSet<string> NumberComparisons { get; } = new HashSet<string>
		{
			ComparisonMap[Comparison.Equal],
			ComparisonMap[Comparison.GreaterEqual],
			ComparisonMap[Comparison.Greater],
			ComparisonMap[Comparison.LessEqual],
			ComparisonMap[Comparison.Less]
		};

		public static HashSet<string> StringComparisons { get; } = new HashSet<string>
		{
			ComparisonMap[Comparison.Equal],
			ComparisonMap[Comparison.Contains]
		};
	}
}