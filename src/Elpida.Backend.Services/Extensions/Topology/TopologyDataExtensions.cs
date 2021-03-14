using System;
using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Extensions.Topology
{
	public static class TopologyDataExtensions
	{
		public static TopologyDto ToDto(this TopologyModel model)
		{
			var topology = new TopologyDto
			{
				TotalDepth = model.TotalDepth,
				TotalLogicalCores = model.TotalLogicalCores,
				TotalPhysicalCores = model.TotalLogicalCores,
				Root = new CpuNodeDto
				{
					Name = model.Root.Name,
					Value = model.Root.Value,
					NodeType = model.Root.NodeType,
					OsIndex = model.Root.OsIndex
				}
			};

			if (model.Root.Children != null)
			{
				topology.Root.Children = new List<CpuNodeDto>();
				foreach (var child in model.Root.Children)
				{
					topology.Root.Children.Add(CreateChild(child));
				}
			}

			if (model.Root.MemoryChildren != null)
			{
				topology.Root.MemoryChildren = new List<CpuNodeDto>();
				foreach (var child in model.Root.MemoryChildren)
				{
					topology.Root.MemoryChildren.Add(CreateChild(child));
				}
			}

			return topology;
		}

		private static CpuNodeDto CreateChild(CpuNodeModel cpuNodeModel)
		{
			return new CpuNodeDto
			{
				Name = cpuNodeModel.Name,
				Value = cpuNodeModel.Value,
				NodeType = cpuNodeModel.NodeType,
				OsIndex = cpuNodeModel.OsIndex,
				Children = cpuNodeModel.Children.Select(CreateChild).ToList(),
				MemoryChildren = cpuNodeModel.MemoryChildren.Select(CreateChild).ToList()
			};
		}

		public static TopologyModel ToModel(this TopologyDto topologyDto, string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("'id' cannot be empty", nameof(id));
			}

			return new TopologyModel
			{
				Id = id,
				TotalDepth = topologyDto.TotalDepth,
				TotalLogicalCores = topologyDto.TotalLogicalCores,
				TotalPhysicalCores = topologyDto.TotalLogicalCores,
				Root = topologyDto.Root.ToModel()
			};
		}
	}
}