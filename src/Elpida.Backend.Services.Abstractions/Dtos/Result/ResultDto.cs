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
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a Benchmark Result.
	/// </summary>
	public class ResultDto : FoundationDto
	{
		/// <summary>
		///     The date and time this result was posted.
		/// </summary>
		public DateTime TimeStamp { get; set; }

		/// <summary>
		///     The Cpu Affinity used by this Benchmark Result.
		/// </summary>
		public IList<long> Affinity { get; set; } = new List<long>();

		/// <summary>
		///     The Elpida Version that this result was produced from.
		/// </summary>
		public ElpidaDto Elpida { get; set; } = new ();

		/// <summary>
		///     The system details for this result.
		/// </summary>
		public SystemDto System { get; set; } = new ();

		/// <summary>
		///     The benchmark result details.
		/// </summary>
		public BenchmarkResultDto Result { get; set; } = new ();
	}
}