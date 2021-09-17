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
using System.ComponentModel.DataAnnotations;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result.Batch
{
	/// <summary>
	///     Represents a specific task result.
	/// </summary>
	public sealed class TaskResultSlimDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TaskResultSlimDto" /> class.
		/// </summary>
		/// <param name="uuid">The UUID of this Task.</param>
		/// <param name="value">The value of the result.</param>
		/// <param name="time">The total time this task run.</param>
		/// <param name="inputSize">How much data this task received as input.</param>
		/// <param name="statistics">The result statistics.</param>
		public TaskResultSlimDto(Guid uuid, double value, double time, long inputSize, TaskRunStatisticsDto statistics)
		{
			Uuid = uuid;
			Value = value;
			Time = time;
			InputSize = inputSize;
			Statistics = statistics;
		}

		/// <summary>
		///     The UUID of this Task.
		/// </summary>
		[Required]
		[NonDefaultValue]
		public Guid Uuid { get; }

		/// <summary>
		///     The value of the result.
		/// </summary>
		[Required]
		[Range(double.Epsilon, double.MaxValue)]
		public double Value { get; }

		/// <summary>
		///     The total time this task run.
		/// </summary>
		[Required]
		[Range(double.Epsilon, double.MaxValue)]
		public double Time { get; }

		/// <summary>
		///     How much data this task received as input.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public long InputSize { get; }

		/// <summary>
		///     The result statistics.
		/// </summary>
		[Required]
		public TaskRunStatisticsDto Statistics { get; }
	}
}