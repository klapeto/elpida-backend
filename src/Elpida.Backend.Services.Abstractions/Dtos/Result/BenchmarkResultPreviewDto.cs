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
	public sealed class BenchmarkResultPreviewDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BenchmarkResultPreviewDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Benchmark Result.</param>
		/// <param name="benchmarkUuid">The UUID of the benchmark.</param>
		/// <param name="timeStamp">The timestamp of the benchmark.</param>
		/// <param name="benchmarkName">The name of the benchmark.</param>
		/// <param name="osName">The operating system name of the system that run the benchmark.</param>
		/// <param name="cpuVendor">The cpu vendor of the system that run the benchmark.</param>
		/// <param name="cpuModelName">The cpu model name of the system that run the benchmark.</param>
		/// <param name="benchmarkScoreUnit">The benchmark score unit.</param>
		/// <param name="score">The benchmark score.</param>
		public BenchmarkResultPreviewDto(
			long id,
			Guid benchmarkUuid,
			DateTime timeStamp,
			string benchmarkName,
			string osName,
			string cpuVendor,
			string cpuModelName,
			string benchmarkScoreUnit,
			double score
		)
			: base(id)
		{
			BenchmarkUuid = benchmarkUuid;
			BenchmarkName = benchmarkName;
			TimeStamp = timeStamp;
			OsName = osName;
			CpuVendor = cpuVendor;
			CpuModelName = cpuModelName;
			BenchmarkScoreUnit = benchmarkScoreUnit;
			Score = score;
		}

		/// <summary>
		///     The UUID of the benchmark.
		/// </summary>
		public Guid BenchmarkUuid { get; }

		/// <summary>
		///     The name of the benchmark.
		/// </summary>
		/// <example>Memory Latency</example>
		public string BenchmarkName { get; }

		/// <summary>
		///     The timestamp of the benchmark.
		/// </summary>
		public DateTime TimeStamp { get; }

		/// <summary>
		///     The operating system name of the system that run the benchmark.
		/// </summary>
		/// <example>Ubuntu</example>
		public string OsName { get; }

		/// <summary>
		///     The cpu vendor of the system that run the benchmark.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; }

		/// <summary>
		///     The cpu model name of the system that run the benchmark.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; }

		/// <summary>
		///     The benchmark score unit.
		/// </summary>
		/// <example>B/s</example>
		public string BenchmarkScoreUnit { get; }

		/// <summary>
		///     The benchmark score.
		/// </summary>
		public double Score { get; }
	}
}