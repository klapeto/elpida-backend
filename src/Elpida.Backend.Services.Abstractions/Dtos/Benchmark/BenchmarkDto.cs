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

namespace Elpida.Backend.Services.Abstractions.Dtos.Benchmark
{
	/// <summary>
	///     The details of a Benchmark.
	/// </summary>
	public sealed class BenchmarkDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BenchmarkDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Benchmark.</param>
		/// <param name="uuid">The UUID of the Benchmark.</param>
		/// <param name="name">The name of the</param>
		/// <param name="scoreSpecification">The score specification details of the Benchmark.</param>
		/// <param name="tasks">The task specifications details of the Benchmark.</param>
		public BenchmarkDto(
			long id,
			Guid uuid,
			string name,
			BenchmarkScoreSpecificationDto scoreSpecification,
			BenchmarkTaskDto[] tasks
		)
			: base(id)
		{
			Uuid = uuid;
			Name = name;
			ScoreSpecification = scoreSpecification;
			Tasks = tasks;
		}

		/// <summary>
		///     The UUID of the Benchmark.
		/// </summary>
		public Guid Uuid { get; }

		/// <summary>
		///     The name of the Benchmark.
		/// </summary>
		/// <example>Test Benchmark.</example>
		public string Name { get; }

		/// <summary>
		///     The score specification details of the Benchmark.
		/// </summary>
		public BenchmarkScoreSpecificationDto ScoreSpecification { get; }

		/// <summary>
		///     The task specifications details of the Benchmark.
		/// </summary>
		public BenchmarkTaskDto[] Tasks { get; }
	}
}