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
	///     Details of a Cpu topology.
	/// </summary>
	public sealed class TopologyDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TopologyDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Topology.</param>
		/// <param name="cpuId">The id of the Cpu this topology belongs.</param>
		/// <param name="cpuVendor">The cpu vendor of this topology.</param>
		/// <param name="cpuModelName">The cpu model name of this topology.</param>
		/// <param name="totalLogicalCores">The total logical cores of this topology.</param>
		/// <param name="totalPhysicalCores">The total physical cores of this topology.</param>
		/// <param name="totalNumaNodes">The total numa nodes of this topology.</param>
		/// <param name="totalPackages">The total cpu packages of this topology.</param>
		/// <param name="root">The first cpu node of this topology.</param>
		public TopologyDto(
			long id,
			long cpuId,
			string? cpuVendor,
			string? cpuModelName,
			int totalLogicalCores,
			int totalPhysicalCores,
			int totalNumaNodes,
			int totalPackages,
			CpuNodeDto root
		)
			: base(id)
		{
			CpuId = cpuId;
			CpuVendor = cpuVendor;
			CpuModelName = cpuModelName;
			TotalLogicalCores = totalLogicalCores;
			TotalPhysicalCores = totalPhysicalCores;
			TotalNumaNodes = totalNumaNodes;
			TotalPackages = totalPackages;
			Root = root;
		}

		/// <summary>
		///     The id of the Cpu this topology belongs.
		/// </summary>
		public long CpuId { get; }

		/// <summary>
		///     The cpu vendor of this topology.
		/// </summary>
		/// <example>ARM</example>
		public string? CpuVendor { get; }

		/// <summary>
		///     The cpu model name of this topology.
		/// </summary>
		/// <example>Cortex A7</example>
		public string? CpuModelName { get; }

		/// <summary>
		///     The total logical cores of this topology.
		/// </summary>
		public int TotalLogicalCores { get; }

		/// <summary>
		///     The total physical cores of this topology.
		/// </summary>
		public int TotalPhysicalCores { get; }

		/// <summary>
		///     The total numa nodes of this topology.
		/// </summary>
		public int TotalNumaNodes { get; }

		/// <summary>
		///     The total cpu packages of this topology.
		/// </summary>
		public int TotalPackages { get; }

		/// <summary>
		///     The first cpu node of this topology.
		/// </summary>
		public CpuNodeDto Root { get; }
	}
}