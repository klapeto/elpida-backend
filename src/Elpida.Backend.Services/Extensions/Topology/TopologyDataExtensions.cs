/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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
        public static TopologyDto ToDto(this TopologyModel topologyModel)
        {
            return new()
            {
                Id = topologyModel.Id,
                CpuId = topologyModel.CpuId,
                CpuVendor = topologyModel.Cpu.Vendor,
                CpuBrand = topologyModel.Cpu.Brand,
                TotalDepth = topologyModel.TotalDepth,
                TotalLogicalCores = topologyModel.TotalLogicalCores,
                TotalPhysicalCores = topologyModel.TotalPhysicalCores,
                TotalMachines = topologyModel.TotalMachines,
                TotalNumaNodes = topologyModel.TotalNumaNodes,
                TotalPackages = topologyModel.TotalPackages,
                Root = JsonConvert.DeserializeObject<CpuNodeDto>(topologyModel.Root)
            };
        }
    }
}