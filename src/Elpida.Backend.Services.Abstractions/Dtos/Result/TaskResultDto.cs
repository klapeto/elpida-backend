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
using Elpida.Backend.Services.Abstractions.Dtos.Task;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a Task result.
	/// </summary>
	public sealed class TaskResultDto : TaskDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TaskResultDto" /> class.
		/// </summary>
		/// <param name="taskId">The id of the Task this result belongs.</param>
		/// <param name="benchmarkResultId">The id of the Benchmark this result belongs.</param>
		/// <param name="cpuId">The id of the Cpu this result belongs.</param>
		/// <param name="topologyId">The id of the Topology this result belongs.</param>
		/// <param name="uuid">The UUID of this Task.</param>
		/// <param name="name">The name of this Task.</param>
		/// <param name="description">The description of this Task.</param>
		/// <param name="result">The result specification of this Task.</param>
		/// <param name="input">The input data specification of this Task.</param>
		/// <param name="output">The output data specification of this Task.</param>
		/// <param name="value">The value of the result.</param>
		/// <param name="time">The total time this task run.</param>
		/// <param name="inputSize">How much data this task received as input.</param>
		/// <param name="statistics">The result statistics.</param>
		public TaskResultDto(
			long taskId,
			long benchmarkResultId,
			long cpuId,
			long topologyId,
			Guid uuid,
			string name,
			string description,
			ResultSpecificationDto result,
			DataSpecificationDto? input,
			DataSpecificationDto? output,
			double value,
			double time,
			long inputSize,
			TaskRunStatisticsDto statistics
		)
			: base(taskId, uuid, name, description, result, input, output)
		{
			BenchmarkResultId = benchmarkResultId;
			CpuId = cpuId;
			TopologyId = topologyId;
			Value = value;
			Time = time;
			InputSize = inputSize;
			Statistics = statistics;
		}

		/// <summary>
		///     The id of the Benchmark this result belongs.
		/// </summary>
		public long BenchmarkResultId { get; }

		/// <summary>
		///     The id of the Cpu this result belongs.
		/// </summary>
		public long CpuId { get; }

		/// <summary>
		///     The id of the Topology this result belongs.
		/// </summary>
		public long TopologyId { get; }

		/// <summary>
		///     The value of the result.
		/// </summary>
		public double Value { get; }

		/// <summary>
		///     The total time this task run.
		/// </summary>
		public double Time { get; }

		/// <summary>
		///     How much data this task received as input.
		/// </summary>
		public long InputSize { get; }

		/// <summary>
		///     The result statistics.
		/// </summary>
		public TaskRunStatisticsDto Statistics { get; }
	}
}