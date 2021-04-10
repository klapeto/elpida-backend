/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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

using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Topology
{
	public static class TopologyDataExtensions
	{
		public static TopologyModel ToModel(this TopologyDto topologyDto)
		{
			var topologyRoot = JsonConvert.SerializeObject(topologyDto.Root);
			return new TopologyModel
			{
				Id = topologyDto.Id,
				CpuId = topologyDto.CpuId,
				TopologyHash = topologyRoot.ToHashString(),
				TotalDepth = topologyDto.TotalDepth,
				TotalLogicalCores = topologyDto.TotalLogicalCores,
				TotalPhysicalCores = topologyDto.TotalLogicalCores,
				Root = topologyRoot
			};
		}
		
		public static TopologyDto ToDto(this TopologyModel topologyModel)
		{
			return new TopologyDto
			{
				Id = topologyModel.Id,
				CpuId = topologyModel.CpuId,
				TotalDepth = topologyModel.TotalDepth,
				TotalLogicalCores = topologyModel.TotalLogicalCores,
				TotalPhysicalCores = topologyModel.TotalLogicalCores,
				Root = JsonConvert.DeserializeObject<CpuNodeDto>(topologyModel.Root)
			};
		}
	}
}