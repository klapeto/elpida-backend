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
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result.Batch
{
	/// <summary>
	///     Contains multiple benchmark results from a system.
	/// </summary>
	public class BenchmarkResultsBatchDto : FoundationDto
	{
		/// <summary>
		///     The Elpida Version that this result was produced from.
		/// </summary>
		public ElpidaDto Elpida { get; set; } = new ();

		/// <summary>
		///     The system details for this result.
		/// </summary>
		public SystemDto System { get; set; } = new ();

		/// <summary>
		///     The benchmark results.
		/// </summary>
		public BenchmarkResultSlimDto[] BenchmarkResults { get; set; } = Array.Empty<BenchmarkResultSlimDto>();
	}
}