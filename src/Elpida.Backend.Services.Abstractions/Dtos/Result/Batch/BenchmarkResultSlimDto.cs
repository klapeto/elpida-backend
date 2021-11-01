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
	///     Describes a single benchmark result.
	/// </summary>
	public sealed class BenchmarkResultSlimDto
	{
		/// <summary>
		///     The Uuid of the benchmark.
		/// </summary>
		[Required]
		[NonDefaultValue]
		public Guid Uuid { get; init; }

		/// <summary>
		///     The timestamp of this benchmark run.
		/// </summary>
		[Required]
		[NonDefaultValue]
		public DateTime Timestamp { get; init; }

		/// <summary>
		///     The affinity used for this benchmark run.
		/// </summary>
		[Required]
		[MinLength(1)]
		public long[] Affinity { get; init; }

		/// <summary>
		///     The score of this benchmark run.
		/// </summary>
		[Required]
		[Range(double.Epsilon, double.MaxValue)]
		public double Score { get; init; }

		/// <summary>
		///     The specific task results.
		/// </summary>
		[Required]
		[MinLength(1)]
		public TaskResultSlimDto[] TaskResults { get; init; }
	}
}