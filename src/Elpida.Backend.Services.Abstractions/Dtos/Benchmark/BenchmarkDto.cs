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

namespace Elpida.Backend.Services.Abstractions.Dtos.Benchmark
{
	/// <summary>
	///     The details of a Benchmark.
	/// </summary>
	public class BenchmarkDto : FoundationDto
	{
		/// <summary>
		///     The UUID of this Benchmark.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		///     The name of this Benchmark.
		/// </summary>
		/// <example>Test Benchmark.</example>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		///     The score specification details of this Benchmark.
		/// </summary>
		public BenchmarkScoreSpecificationDto ScoreSpecification { get; set; } = new ();

		/// <summary>
		///     The task specifications details of this Benchmark.
		/// </summary>
		public IList<BenchmarkTaskDto> Tasks { get; set; } = new List<BenchmarkTaskDto>();
	}
}