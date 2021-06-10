/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

using Elpida.Backend.Data.Abstractions.Models.Task;

namespace Elpida.Backend.Data.Abstractions.Models.Benchmark
{
	public class BenchmarkTaskModel : Entity
	{
		public long BenchmarkId { get; set; }

		public BenchmarkModel Benchmark { get; set; } = default!;

		public long TaskId { get; set; }

		public TaskModel Task { get; set; } = default!;

		public bool CanBeDisabled { get; set; }

		public long IterationsToRun { get; set; }

		public bool IsCountedOnResults { get; set; }

		public bool CanBeMultiThreaded { get; set; }
	}
}