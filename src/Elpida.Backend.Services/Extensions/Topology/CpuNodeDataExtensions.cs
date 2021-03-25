using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Extensions.Topology
{
	public static class CpuNodeDataExtensions
	{
		// public static CpuNodeDto ToDto(this CpuNodeModel model)
		// {
		// 	return new CpuNodeDto
		// 	{
		// 		Name = model.Name,
		// 		Value = model.Value,
		// 		NodeType = model.NodeType,
		// 		OsIndex = model.OsIndex,
		// 		Children = model.Children?.Select(c => c.ToDto()).ToList(),
		// 		MemoryChildren = model.MemoryChildren?.Select(c => c.ToDto()).ToList()
		// 	};
		// }
		//
		// public static CpuNodeModel ToModel(this CpuNodeDto cpuNodeDto)
		// {
		// 	return new CpuNodeModel
		// 	{
		// 		Name = cpuNodeDto.Name,
		// 		Value = cpuNodeDto.Value,
		// 		NodeType = cpuNodeDto.NodeType,
		// 		OsIndex = cpuNodeDto.OsIndex,
		// 		Children = cpuNodeDto.Children?.Select(c => c.ToModel()).ToList(),
		// 		MemoryChildren = cpuNodeDto.MemoryChildren?.Select(c => c.ToModel()).ToList()
		// 	};
		// }
	}
}