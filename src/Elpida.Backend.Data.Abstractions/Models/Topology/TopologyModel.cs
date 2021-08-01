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

using Elpida.Backend.Data.Abstractions.Models.Cpu;

namespace Elpida.Backend.Data.Abstractions.Models.Topology
{
	public class TopologyModel : Entity
	{
		public long CpuId { get; set; }

		public CpuModel Cpu { get; set; } = default!;

		public string TopologyHash { get; set; } = default!;

		public int TotalLogicalCores { get; set; }

		public int TotalPhysicalCores { get; set; }

		public int TotalNumaNodes { get; set; }

		public int TotalPackages { get; set; }

		public int TotalDepth { get; set; }

		public string Root { get; set; } = default!;
	}
}