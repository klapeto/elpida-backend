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
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a Benchmark Result.
	/// </summary>
	public sealed class ResultDto : FoundationDto
	{
		/// <summary>
		///     The date and time this result was posted.
		/// </summary>
		public DateTime TimeStamp { get; init; }

		/// <summary>
		///     The Cpu Affinity used by this Benchmark Result.
		/// </summary>
		public long[] Affinity { get; init; }

		/// <summary>
		///     The Elpida Version that this result was produced from.
		/// </summary>
		public ElpidaVersionDto ElpidaVersion { get; init; }

		/// <summary>
		///     The system details for this result.
		/// </summary>
		public SystemDto System { get; init; }

		/// <summary>
		///     The score of the benchmark.
		/// </summary>
		public double Score { get; init; }

		/// <summary>
		///     The UUID of this Benchmark.
		/// </summary>
		public Guid Uuid { get; init; }

		/// <summary>
		///     The name of this Benchmark.
		/// </summary>
		/// <example>Test Benchmark.</example>
		public string Name { get; init; }

		/// <summary>
		///     The score specification details of this Benchmark.
		/// </summary>
		public BenchmarkScoreSpecificationDto ScoreSpecification { get; init; }

		/// <summary>
		///     The specific Task results.
		/// </summary>
		public TaskResultDto[] TaskResults { get; init; }
	}
}