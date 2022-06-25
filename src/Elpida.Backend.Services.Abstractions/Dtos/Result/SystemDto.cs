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
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a system.
	/// </summary>
	public sealed class SystemDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="SystemDto" /> class.
		/// </summary>
		/// <param name="cpu">The Cpu of this system.</param>
		/// <param name="os">The operating system of this system.</param>
		/// <param name="topology">The topology of this system.</param>
		/// <param name="memory">The memory details of this system.</param>
		/// <param name="timing">Timing details of this system.</param>
		public SystemDto(CpuDto cpu, OperatingSystemDto os, TopologyDto topology, MemoryDto memory, TimingDto timing)
		{
			Cpu = cpu;
			Os = os;
			Topology = topology;
			Memory = memory;
			Timing = timing;
		}

		/// <summary>
		///     The Cpu of this system.
		/// </summary>
		[Required]
		public CpuDto Cpu { get; }

		/// <summary>
		///     The operating system of this system.
		/// </summary>
		[Required]
		public OperatingSystemDto Os { get; }

		/// <summary>
		///     The topology of this system.
		/// </summary>
		[Required]
		public TopologyDto Topology { get; }

		/// <summary>
		///     The memory details of this system.
		/// </summary>
		[Required]
		public MemoryDto Memory { get; }

		/// <summary>
		///     Timing details of this system.
		/// </summary>
		[Required]
		public TimingDto Timing { get; }
	}
}