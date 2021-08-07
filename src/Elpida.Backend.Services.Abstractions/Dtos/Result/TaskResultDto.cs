/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2020 Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Elpida.Backend.Services.Abstractions.Dtos.Task;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a Task result.
	/// </summary>
	public class TaskResultDto : TaskDto
	{
		/// <summary>
		///     The id of the Task this result belongs.
		/// </summary>
		public long TaskId { get; set; }

		/// <summary>
		///     The id of the Benchmark this result belongs..
		/// </summary>
		public long BenchmarkResultId { get; set; }

		/// <summary>
		///     The id of the Cpu this result belongs.
		/// </summary>
		public long CpuId { get; set; }

		/// <summary>
		///     The id of the Topology this result belongs.
		/// </summary>
		public long TopologyId { get; set; }

		/// <summary>
		///     The value of the result.
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		///     The total time this task run.
		/// </summary>
		public double Time { get; set; }

		/// <summary>
		///     How much data this task received as input.
		/// </summary>
		public long InputSize { get; set; }

		/// <summary>
		///     The result statistics.
		/// </summary>
		public TaskRunStatisticsDto Statistics { get; set; } = new ();
	}
}