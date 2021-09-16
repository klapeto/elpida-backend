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
	public sealed class BenchmarkResultDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BenchmarkResultDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Benchmark Result.</param>
		/// <param name="timeStamp">The date and time this result was posted.</param>
		/// <param name="uuid">The UUID of this Benchmark.</param>
		/// <param name="name">The name of this Benchmark.</param>
		/// <param name="affinity">The Cpu Affinity used by this Benchmark Result.</param>
		/// <param name="elpidaVersion">The Elpida Version that this result was produced from.</param>
		/// <param name="system">The system details for this result.</param>
		/// <param name="score">The score of the benchmark.</param>
		/// <param name="scoreSpecification">The score specification details of this Benchmark.</param>
		/// <param name="taskResults">The specific Task results.</param>
		public BenchmarkResultDto(
			long id,
			DateTime timeStamp,
			Guid uuid,
			string name,
			long[] affinity,
			ElpidaVersionDto elpidaVersion,
			SystemDto system,
			double score,
			BenchmarkScoreSpecificationDto scoreSpecification,
			TaskResultDto[] taskResults
		)
			: base(id)
		{
			TimeStamp = timeStamp;
			Affinity = affinity;
			ElpidaVersion = elpidaVersion;
			System = system;
			Score = score;
			Uuid = uuid;
			Name = name;
			ScoreSpecification = scoreSpecification;
			TaskResults = taskResults;
		}

		/// <summary>
		///     The date and time this result was posted.
		/// </summary>
		public DateTime TimeStamp { get; }

		/// <summary>
		///     The Cpu Affinity used by this Benchmark Result.
		/// </summary>
		public long[] Affinity { get; }

		/// <summary>
		///     The Elpida Version that this result was produced from.
		/// </summary>
		public ElpidaVersionDto ElpidaVersion { get; }

		/// <summary>
		///     The system details for this result.
		/// </summary>
		public SystemDto System { get; }

		/// <summary>
		///     The score of the benchmark.
		/// </summary>
		public double Score { get; }

		/// <summary>
		///     The UUID of this Benchmark.
		/// </summary>
		public Guid Uuid { get; }

		/// <summary>
		///     The name of this Benchmark.
		/// </summary>
		/// <example>Test Benchmark.</example>
		public string Name { get; }

		/// <summary>
		///     The score specification details of this Benchmark.
		/// </summary>
		public BenchmarkScoreSpecificationDto ScoreSpecification { get; }

		/// <summary>
		///     The specific Task results.
		/// </summary>
		public TaskResultDto[] TaskResults { get; }
	}
}