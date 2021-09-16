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
		///     Initializes a new instance of the <see cref="BenchmarkStatisticsDto" /> class.
		/// </summary>
		/// <param name="id">The id of the statistic.</param>
		/// <param name="cpu">The Cpu this statistic represents.</param>
		/// <param name="benchmark">The Benchmark this statistic represents.</param>
		/// <param name="sampleSize">The sample size for this benchmark/cpu combination.</param>
		/// <param name="max">The maximum score for this benchmark/cpu combination.</param>
		/// <param name="min">The minimum score for this benchmark/cpu combination.</param>
		/// <param name="mean">The mean score for this benchmark/cpu combination.</param>
		/// <param name="standardDeviation">The standard deviation of the score for this benchmark/cpu combination.</param>
		/// <param name="tau">The tau of the score for this benchmark/cpu combination.</param>
		/// <param name="marginOfError">The margin of error of the score for this benchmark/cpu combination.</param>
		/// <param name="classes">The frequency classes benchmark/cpu combination.</param>
		public BenchmarkStatisticsDto(
			long id,
			CpuDto cpu,
			BenchmarkDto benchmark,
			long sampleSize,
			double max,
			double min,
			double mean,
			double standardDeviation,
			double tau,
			double marginOfError,
			FrequencyClassDto[] classes
		)
			: base(id)
		{
			Cpu = cpu;
			Benchmark = benchmark;
			SampleSize = sampleSize;
			Max = max;
			Min = min;
			Mean = mean;
			StandardDeviation = standardDeviation;
			Tau = tau;
			MarginOfError = marginOfError;
			Classes = classes;
		}

		/// <summary>
		///     The Cpu this statistic represents.
		/// </summary>
		public CpuDto Cpu { get; }

		/// <summary>
		///     The Benchmark this statistic represents.
		/// </summary>
		public BenchmarkDto Benchmark { get; }

		/// <summary>
		///     The sample size for this benchmark/cpu combination.
		/// </summary>
		public long SampleSize { get; }

		/// <summary>
		///     The maximum score for this benchmark/cpu combination.
		/// </summary>
		public double Max { get; }

		/// <summary>
		///     The minimum score for this benchmark/cpu combination.
		/// </summary>
		public double Min { get; }

		/// <summary>
		///     The mean score for this benchmark/cpu combination.
		/// </summary>
		public double Mean { get; }

		/// <summary>
		///     The standard deviation of the score for this benchmark/cpu combination.
		/// </summary>
		public double StandardDeviation { get; }

		/// <summary>
		///     The tau of the score for this benchmark/cpu combination.
		/// </summary>
		public double Tau { get; }

		/// <summary>
		///     The margin of error of the score for this benchmark/cpu combination.
		/// </summary>
		public double MarginOfError { get; }

		/// <summary>
		///     The frequency classes benchmark/cpu combination.
		/// </summary>
		public FrequencyClassDto[] Classes { get; }
	}
}