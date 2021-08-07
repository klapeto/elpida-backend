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

using System;
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a benchmark result.
	/// </summary>
	public class BenchmarkResultDto : FoundationDto
	{
		/// <summary>
		///     The UUID of the Benchmark.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		///     The name of the benchmark.
		/// </summary>
		/// <example>DRAM Latency.</example>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		///     The benchmark score specification.
		/// </summary>
		public BenchmarkScoreSpecificationDto ScoreSpecification { get; set; } = new ();

		/// <summary>
		///     The score of the benchmark.
		/// </summary>
		public double Score { get; set; }

		/// <summary>
		///     The specific Task results.
		/// </summary>
		public IList<TaskResultDto> TaskResults { get; set; } = new List<TaskResultDto>();
	}
}