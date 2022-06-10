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

using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     A preview of a Benchmark result.
	/// </summary>
	public sealed class ResultPreviewDto : FoundationDto
	{
		/// <summary>
		///     The UUID of the benchmark.
		/// </summary>
		public Guid BenchmarkUuid { get; init; }

		/// <summary>
		///     The name of the benchmark.
		/// </summary>
		/// <example>Memory Latency</example>
		public string BenchmarkName { get; init; }

		/// <summary>
		///     The timestamp of the benchmark.
		/// </summary>
		public DateTime TimeStamp { get; init; }

		/// <summary>
		///     The operating system name of the system that run the benchmark.
		/// </summary>
		/// <example>Ubuntu</example>
		public string OsName { get; init; }

		/// <summary>
		///     The cpu vendor of the system that run the benchmark.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; init; }

		/// <summary>
		///     The cpu model name of the system that run the benchmark.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; init; }

		/// <summary>
		///     The benchmark score unit.
		/// </summary>
		/// <example>B/s</example>
		public string BenchmarkScoreUnit { get; init; }

		/// <summary>
		///     The benchmark score.
		/// </summary>
		public double Score { get; init; }
	}
}