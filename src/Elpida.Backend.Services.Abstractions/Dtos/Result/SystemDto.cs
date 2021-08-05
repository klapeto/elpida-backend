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

using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of a system.
	/// </summary>
	public class SystemDto
	{
		/// <summary>
		///     The Cpu of this system.
		/// </summary>
		public CpuDto Cpu { get; set; } = new ();

		/// <summary>
		///     The operating system of this system.
		/// </summary>
		public OsDto Os { get; set; } = new ();

		/// <summary>
		///     The topology of this system.
		/// </summary>
		public TopologyDto Topology { get; set; } = new ();

		/// <summary>
		///     The memory details of this system.
		/// </summary>
		public MemoryDto Memory { get; set; } = new ();

		/// <summary>
		///     Timing details of this system.
		/// </summary>
		public TimingDto Timing { get; set; } = new ();
	}
}