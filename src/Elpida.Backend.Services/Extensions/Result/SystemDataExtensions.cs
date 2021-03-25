using System;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Extensions.Topology;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class SystemDataExtensions
	{
		// 	public static SystemDto ToDto(this SystemModel model, CpuModel cpuModel, TopologyModel topologyModel)
		// 	{
		// 		return new SystemDto
		// 		{
		// 			Os = model.Os.ToDto(),
		// 			Cpu = cpuModel.ToDto(),
		// 			Timing = model.Timing.ToDto(),
		// 			Memory = model.Memory.ToDto(),
		// 			Topology = topologyModel.ToDto(),
		// 		};
		// 	}
		//
		// 	public static SystemModel ToModel(this SystemDto dto, int cpuId, int topologyId)
		// 	{
		// 		return new SystemModel
		// 		{
		// 			Memory = dto.Memory.ToModel(),
		// 			Os = dto.Os.ToModel(),
		// 			Timing = dto.Timing.ToModel(),
		// 			CpuId = cpuId,
		// 			TopologyId = topologyId
		// 		};
		// 	}
	}
}