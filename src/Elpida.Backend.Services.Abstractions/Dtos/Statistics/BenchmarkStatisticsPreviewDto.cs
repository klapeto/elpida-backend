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
		///     The cpu vendor for this statistic.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; init; }

		/// <summary>
		///     The cpu model name for this statistic.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; init; }

		/// <summary>
		///     The benchmark name for this statistic.
		/// </summary>
		/// <example>Memory read bandwidth</example>
		public string BenchmarkName { get; init; }

		/// <summary>
		///     The benchmark score unit for this statistic.
		/// </summary>
		/// <example>B/s</example>
		public string BenchmarkScoreUnit { get; init; }

		/// <summary>
		///     The benchmark UUID for this statistic.
		/// </summary>
		public Guid BenchmarkUuid { get; init; }

		/// <summary>
		///     The benchmark statistic mean score.
		/// </summary>
		public double Mean { get; init; }

		/// <summary>
		///     The sample size for this benchmark.
		/// </summary>
		public long SampleSize { get; init; }

		/// <summary>
		///     The value comparison type for this benchmark.
		/// </summary>
		public ValueComparison Comparison { get; init; }
	}
}