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

using System.ComponentModel.DataAnnotations;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Statistics of a Task run result.
	/// </summary>
	public sealed class TaskRunStatisticsDto
	{
		/// <summary>
		///     How many times this task run.
		/// </summary>
		[Required]
		[Range(1, long.MaxValue)]
		public long SampleSize { get; init; }

		/// <summary>
		///     The maximum value of the result.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public double Max { get; init; }

		/// <summary>
		///     The minimum value of the result.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public double Min { get; init; }

		/// <summary>
		///     The mean value of the result.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public double Mean { get; init; }

		/// <summary>
		///     The standard deviation of the value of the result.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public double StandardDeviation { get; init; }

		/// <summary>
		///     The tau of this sample size.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public double Tau { get; init; }

		/// <summary>
		///     The margin of error of the value of the result.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public double MarginOfError { get; init; }
	}
}