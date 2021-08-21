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
using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result.Batch
{
	/// <summary>
	///     Describes a single benchmark result.
	/// </summary>
	public class BenchmarkResultSlimDto
	{
		/// <summary>
		///     The Uuid of the benchmark.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		///     The timestamp of this benchmark run.
		/// </summary>
		public DateTime Timestamp { get; set; }

		/// <summary>
		///     The affinity used for this benchmark run.
		/// </summary>
		public long[] Affinity { get; set; } = Array.Empty<long>();

		/// <summary>
		///     The score of this benchmark run.
		/// </summary>
		public double Score { get; set; }

		/// <summary>
		///     The specific task results.
		/// </summary>
		public IList<TaskResultSlimDto> TaskResults { get; set; } = new List<TaskResultSlimDto>();
	}
}