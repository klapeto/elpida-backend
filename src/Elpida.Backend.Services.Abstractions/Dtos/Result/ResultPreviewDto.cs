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

using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     A preview of a Benchmark result.
	/// </summary>
	public class ResultPreviewDto : FoundationDto
	{
		/// <summary>
		///     The UUID of the benchmark.
		/// </summary>
		public Guid BenchmarkUuid { get; set; }

		/// <summary>
		///     The name of the benchmark.
		/// </summary>
		/// <example>Memory Latency</example>
		public string BenchmarkName { get; set; } = string.Empty;

		/// <summary>
		///     The timestamp of the benchmark when posted.
		/// </summary>
		public DateTime TimeStamp { get; set; }

		/// <summary>
		///     The major version of Elpida.
		/// </summary>
		public int ElpidaVersionMajor { get; set; }

		/// <summary>
		///     The minor version of Elpida.
		/// </summary>
		public int ElpidaVersionMinor { get; set; }

		/// <summary>
		///     The revision version of Elpida.
		/// </summary>
		public int ElpidaVersionRevision { get; set; }

		/// <summary>
		///     The build number of Elpida.
		/// </summary>
		public int ElpidaVersionBuild { get; set; }

		/// <summary>
		///     The operating system name of the system that run the benchmark.
		/// </summary>
		/// <example>Ubuntu</example>
		public string OsName { get; set; } = string.Empty;

		/// <summary>
		///     The operating system version of the system that run the benchmark.
		/// </summary>
		/// <example>21.04</example>
		public string OsVersion { get; set; } = string.Empty;

		/// <summary>
		///     The cpu vendor of the system that run the benchmark.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; set; } = string.Empty;

		/// <summary>
		///     The cpu model name of the system that run the benchmark.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; set; } = string.Empty;

		/// <summary>
		///     The cpu frequency of the system that run the benchmark.
		/// </summary>
		public long CpuFrequency { get; set; }

		/// <summary>
		///     The cpu core count of the system that run the benchmark.
		/// </summary>
		public int CpuCores { get; set; }

		/// <summary>
		///     The cpu logical cores of the system that run the benchmark.
		/// </summary>
		public int CpuLogicalCores { get; set; }

		/// <summary>
		///     The memory size of the system that run the benchmark.
		/// </summary>
		public long MemorySize { get; set; }
	}
}