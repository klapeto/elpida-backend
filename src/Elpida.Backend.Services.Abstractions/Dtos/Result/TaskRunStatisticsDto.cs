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

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Statistics of a Task run result.
	/// </summary>
	public sealed class TaskRunStatisticsDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TaskRunStatisticsDto" /> class.
		/// </summary>
		/// <param name="sampleSize">How many times this task run.</param>
		/// <param name="max">The maximum value of the result.</param>
		/// <param name="min">The minimum value of the result.</param>
		/// <param name="mean">The mean value of the result.</param>
		/// <param name="standardDeviation">The standard deviation of the value of the result.</param>
		/// <param name="tau">The tau of this sample size.</param>
		/// <param name="marginOfError">The margin of error of the value of the result.</param>
		public TaskRunStatisticsDto(
			long sampleSize,
			double max,
			double min,
			double mean,
			double standardDeviation,
			double tau,
			double marginOfError
		)
		{
			SampleSize = sampleSize;
			Max = max;
			Min = min;
			Mean = mean;
			StandardDeviation = standardDeviation;
			Tau = tau;
			MarginOfError = marginOfError;
		}

		/// <summary>
		///     How many times this task run.
		/// </summary>
		public long SampleSize { get; }

		/// <summary>
		///     The maximum value of the result.
		/// </summary>
		public double Max { get; }

		/// <summary>
		///     The minimum value of the result.
		/// </summary>
		public double Min { get; }

		/// <summary>
		///     The mean value of the result.
		/// </summary>
		public double Mean { get; }

		/// <summary>
		///     The standard deviation of the value of the result.
		/// </summary>
		public double StandardDeviation { get; }

		/// <summary>
		///     The tau of this sample size.
		/// </summary>
		public double Tau { get; }

		/// <summary>
		///     The margin of error of the value of the result.
		/// </summary>
		public double MarginOfError { get; }
	}
}