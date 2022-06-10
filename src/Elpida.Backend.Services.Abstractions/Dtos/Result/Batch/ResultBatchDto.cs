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

using System.ComponentModel.DataAnnotations;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result.Batch
{
	/// <summary>
	///     Contains multiple benchmark results from a system.
	/// </summary>
	public sealed class ResultBatchDto : FoundationDto
	{
		/// <summary>
		///     The Elpida Version that this result was produced from.
		/// </summary>
		[Required]
		public ElpidaVersionDto ElpidaVersion { get; init; }

		/// <summary>
		///     The system details for this result.
		/// </summary>
		[Required]
		public SystemDto System { get; init; }

		/// <summary>
		///     The benchmark results.
		/// </summary>
		[Required]
		[MinLength(1)]
		public BenchmarkResultSlimDto[] BenchmarkResults { get; init; }
	}
}