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
using Elpida.Backend.Common;

namespace Elpida.Backend.Services.Abstractions.Dtos.Statistics
{
	/// <summary>
	///     Preview data for Benchmark statistics.
	/// </summary>
	public sealed class BenchmarkStatisticsPreviewDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BenchmarkStatisticsPreviewDto" /> class.
		/// </summary>
		/// <param name="id">The id of the statistic.</param>
		/// <param name="cpuVendor">The cpu vendor for this statistic.</param>
		/// <param name="cpuModelName">The cpu model name for this statistic.</param>
		/// <param name="benchmarkUuid">The benchmark UUID for this statistic.</param>
		/// <param name="benchmarkName">The benchmark name for this statistic.</param>
		/// <param name="benchmarkScoreUnit">The benchmark score unit for this statistic.</param>
		/// <param name="mean">The benchmark statistic mean score.</param>
		/// <param name="sampleSize">The sample size for this benchmark.</param>
		/// <param name="comparison">The value comparison type for this benchmark.</param>
		public BenchmarkStatisticsPreviewDto(
			long id,
			string cpuVendor,
			string cpuModelName,
			Guid benchmarkUuid,
			string benchmarkName,
			string benchmarkScoreUnit,
			double mean,
			long sampleSize,
			ValueComparison comparison
		)
			: base(id)
		{
			CpuVendor = cpuVendor;
			CpuModelName = cpuModelName;
			BenchmarkName = benchmarkName;
			BenchmarkScoreUnit = benchmarkScoreUnit;
			BenchmarkUuid = benchmarkUuid;
			Mean = mean;
			SampleSize = sampleSize;
			Comparison = comparison;
		}

		/// <summary>
		///     The cpu vendor for this statistic.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; }

		/// <summary>
		///     The cpu model name for this statistic.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; }

		/// <summary>
		///     The benchmark name for this statistic.
		/// </summary>
		/// <example>Memory read bandwidth</example>
		public string BenchmarkName { get; }

		/// <summary>
		///     The benchmark score unit for this statistic.
		/// </summary>
		/// <example>B/s</example>
		public string BenchmarkScoreUnit { get; }

		/// <summary>
		///     The benchmark UUID for this statistic.
		/// </summary>
		public Guid BenchmarkUuid { get; }

		/// <summary>
		///     The benchmark statistic mean score.
		/// </summary>
		public double Mean { get; }

		/// <summary>
		///     The sample size for this benchmark.
		/// </summary>
		public long SampleSize { get; }

		/// <summary>
		///     The value comparison type for this benchmark.
		/// </summary>
		public ValueComparison Comparison { get; }
	}
}