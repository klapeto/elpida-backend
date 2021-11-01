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
	public sealed class TopologyPreviewDto : FoundationDto
	{
		/// <summary>
		///     The id of the cpu this topology belongs.
		/// </summary>
		public long CpuId { get; init; }

		/// <summary>
		///     The vendor of the cpu this topology belongs.
		/// </summary>
		/// <example>ARM</example>
		public string CpuVendor { get; init; }

		/// <summary>
		///     The cpu model name of the cpu this topology belongs.
		/// </summary>
		/// <example>Cortex A7</example>
		public string CpuModelName { get; init; }

		/// <summary>
		///     The total logical cores of this topology.
		/// </summary>
		public int TotalLogicalCores { get; init; }

		/// <summary>
		///     The total physical cores of this topology.
		/// </summary>
		public int TotalPhysicalCores { get; init; }

		/// <summary>
		///     The numa nodes of this topology.
		/// </summary>
		public int TotalNumaNodes { get; init; }

		/// <summary>
		///     The total packages of this topology.
		/// </summary>
		public int TotalPackages { get; init; }

		/// <summary>
		///     The hash of this topology.
		/// </summary>
		public string Hash { get; init; }
	}
}