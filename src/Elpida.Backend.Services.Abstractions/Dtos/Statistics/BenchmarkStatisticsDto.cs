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

using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;

namespace Elpida.Backend.Services.Abstractions.Dtos.Statistics
{
	/// <summary>
	///     Statistics for a benchmark/cpu combination.
	/// </summary>
	public sealed class BenchmarkStatisticsDto : FoundationDto
	{
		/// <summary>
		///     The Cpu this statistic represents.
		/// </summary>
		public CpuDto Cpu { get; init; }

		/// <summary>
		///     The Benchmark this statistic represents.
		/// </summary>
		public BenchmarkDto Benchmark { get; init; }

		/// <summary>
		///     The sample size for this benchmark/cpu combination.
		/// </summary>
		public long SampleSize { get; init; }

		/// <summary>
		///     The maximum score for this benchmark/cpu combination.
		/// </summary>
		public double Max { get; init; }

		/// <summary>
		///     The minimum score for this benchmark/cpu combination.
		/// </summary>
		public double Min { get; init; }

		/// <summary>
		///     The mean score for this benchmark/cpu combination.
		/// </summary>
		public double Mean { get; init; }

		/// <summary>
		///     The standard deviation of the score for this benchmark/cpu combination.
		/// </summary>
		public double StandardDeviation { get; init; }

		/// <summary>
		///     The tau of the score for this benchmark/cpu combination.
		/// </summary>
		public double Tau { get; init; }

		/// <summary>
		///     The margin of error of the score for this benchmark/cpu combination.
		/// </summary>
		public double MarginOfError { get; init; }

		/// <summary>
		///     The frequency classes benchmark/cpu combination.
		/// </summary>
		public FrequencyClassDto[] Classes { get; init; }
	}
}