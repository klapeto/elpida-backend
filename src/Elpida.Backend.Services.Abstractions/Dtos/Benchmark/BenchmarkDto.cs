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
		///     The UUID of the Benchmark.
		/// </summary>
		public Guid Uuid { get; init; }

		/// <summary>
		///     The name of the Benchmark.
		/// </summary>
		/// <example>Test Benchmark.</example>
		public string Name { get; init; }

		/// <summary>
		///     The score specification details of the Benchmark.
		/// </summary>
		public BenchmarkScoreSpecificationDto ScoreSpecification { get; init; }

		/// <summary>
		///     The task specifications details of the Benchmark.
		/// </summary>
		public BenchmarkTaskDto[] Tasks { get; init; }
	}
}