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

namespace Elpida.Backend.Services.Abstractions.Dtos.Topology
{
	/// <summary>
	///     A preview of a topology.
	/// </summary>
	public class TopologyPreviewDto : FoundationDto
	{
		/// <summary>
		///     The id of the cpu this topology belongs.
		/// </summary>
		public long CpuId { get; set; }

		/// <summary>
		///     The vendor of the cpu this topology belongs.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; set; } = default!;

		/// <summary>
		///     The cpu model name of the cpu this topology belongs.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; set; } = default!;

		/// <summary>
		///     The total logical cores of this topology.
		/// </summary>
		public int TotalLogicalCores { get; set; }

		/// <summary>
		///     The total physical cores of this topology.
		/// </summary>
		public int TotalPhysicalCores { get; set; }

		/// <summary>
		///     The numa nodes of this topology.
		/// </summary>
		public int TotalNumaNodes { get; set; }

		/// <summary>
		///     The total packages of this topology.
		/// </summary>
		public int TotalPackages { get; set; }

		/// <summary>
		///     The total depth of this topology.
		/// </summary>
		public int TotalDepth { get; set; }

		/// <summary>
		///     The hash of this topology.
		/// </summary>
		public string Hash { get; set; } = default!;
	}
}