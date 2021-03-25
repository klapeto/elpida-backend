using System;
using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Topology
{
	public static class TopologyDataExtensions
	{
		public static TopologyDto ToDto(this TopologyModel model)
		{
			return new TopologyDto
			{
				TotalDepth = model.TotalDepth,
				TotalLogicalCores = model.TotalLogicalCores,
				TotalPhysicalCores = model.TotalLogicalCores,
				Root = JsonConvert.DeserializeObject<CpuNodeDto>(model.Root)
			};
		}

		public static TopologyModel ToModel(this TopologyDto topologyDto, 
			CpuModel cpu,
			string topologyRoot,
			string topologyHash,
			long id)
		{
			return new TopologyModel
			{
				Id = id,
				Cpu = cpu,
				TopologyHash = topologyHash,
				TotalDepth = topologyDto.TotalDepth,
				TotalLogicalCores = topologyDto.TotalLogicalCores,
				TotalPhysicalCores = topologyDto.TotalLogicalCores,
				Root = topologyRoot
			};
		}
	}
}