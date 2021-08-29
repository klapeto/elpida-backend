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

namespace Elpida.Backend.Services.Abstractions.Dtos.Benchmark
{
	/// <summary>
	///     Details of a Task instance of a Benchmark.
	/// </summary>
	public sealed class BenchmarkTaskDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BenchmarkTaskDto" /> class.
		/// </summary>
		/// <param name="uuid">The Task UUID.</param>
		/// <param name="task">The Task details.</param>
		/// <param name="canBeMultiThreaded">If this Task instance is allowed to run multi threaded.</param>
		/// <param name="canBeDisabled">If this Task instance is allowed to be disabled.</param>
		/// <param name="iterationsToRun">How many iterations this Task instance will be run.</param>
		/// <param name="isCountedOnResults">If the results of this Task instance are counted on the Benchmark score etc.</param>
		public BenchmarkTaskDto(
			Guid uuid,
			TaskDto? task,
			bool canBeMultiThreaded,
			bool canBeDisabled,
			long iterationsToRun,
			bool isCountedOnResults
		)
		{
			Uuid = uuid;
			Task = task;
			CanBeMultiThreaded = canBeMultiThreaded;
			CanBeDisabled = canBeDisabled;
			IterationsToRun = iterationsToRun;
			IsCountedOnResults = isCountedOnResults;
		}

		/// <summary>
		///     The Task UUID.
		/// </summary>
		public Guid Uuid { get; }

		/// <summary>
		///     The Task details.
		/// </summary>
		public TaskDto? Task { get; }

		/// <summary>
		///     If this Task instance is allowed to run multi threaded.
		/// </summary>
		public bool CanBeMultiThreaded { get; }

		/// <summary>
		///     If this Task instance is allowed to be disabled.
		/// </summary>
		public bool CanBeDisabled { get; }

		/// <summary>
		///     How many iterations this Task instance will be run.
		/// </summary>
		/// <example>1</example>
		public long IterationsToRun { get; }

		/// <summary>
		///     If the results of this Task instance are counted on the Benchmark score etc.
		/// </summary>
		public bool IsCountedOnResults { get; }
	}
}