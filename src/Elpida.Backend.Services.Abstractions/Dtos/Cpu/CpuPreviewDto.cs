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

namespace Elpida.Backend.Services.Abstractions.Dtos.Cpu
{
	/// <summary>
	///     Represents a Cpu.
	/// </summary>
	public class CpuPreviewDto : FoundationDto
	{
		/// <summary>
		///     The vendor name of the cpu.
		/// </summary>
		/// <example>ARM</example>
		public string Vendor { get; set; } = default!;

		/// <summary>
		///     The model name of the cpu.
		/// </summary>
		/// <example>Cortex A7</example>
		public string ModelName { get; set; } = default!;

		/// <summary>
		///     The number of topologies this cpu has.
		/// </summary>
		public int TopologiesCount { get; set; }

		/// <summary>
		///     The number of
		/// </summary>
		public int TaskStatisticsCount { get; set; }
	}
}