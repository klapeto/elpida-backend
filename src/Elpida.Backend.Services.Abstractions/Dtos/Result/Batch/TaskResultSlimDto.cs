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
		///     The UUID of this Task.
		/// </summary>
		[Required]
		[NonDefaultValue]
		public Guid Uuid { get; init; }

		/// <summary>
		///     The value of the result.
		/// </summary>
		[Required]
		[Range(double.Epsilon, double.MaxValue)]
		public double Value { get; init; }

		/// <summary>
		///     The total time this task run.
		/// </summary>
		[Required]
		[Range(double.Epsilon, double.MaxValue)]
		public double Time { get; init; }

		/// <summary>
		///     How much data this task received as input.
		/// </summary>
		[Required]
		[Range(0.0, double.MaxValue)]
		public long InputSize { get; init; }

		/// <summary>
		///     The result statistics.
		/// </summary>
		[Required]
		public TaskRunStatisticsDto Statistics { get; init; }
	}
}