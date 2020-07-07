using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services
{
	public static class ResultDtoExtensions
	{
		public static ResultModel ToResultModel(this ResultDto resultDto)
		{
			var resultModel = new ResultModel
			{
				Id = resultDto.Id,
				TimeStamp = resultDto.TimeStamp,
				Elpida =
					new ElpidaModel
					{
						Compiler = new CompilerModel
						{
							Name = resultDto.Elpida.Compiler.Name,
							Version = resultDto.Elpida.Compiler.Version
						},
						Version = new VersionModel
						{
							Build = resultDto.Elpida.Version.Build,
							Major = resultDto.Elpida.Version.Build,
							Minor = resultDto.Elpida.Version.Minor,
							Revision = resultDto.Elpida.Version.Revision
						}
					},
				Result =
					new BenchmarkResultModel
					{
						Name = resultDto.Result.Name,
						TaskResults = resultDto.Result.TaskResults.Select(d => new TaskResultModel
						{
							Description = d.Description,
							Name = d.Name,
							Suffix = d.Suffix,
							Time = d.Time,
							Type = d.Type,
							Value = d.Value,
							InputSize = d.InputSize
						}).ToList()
					},
				Affinity = resultDto.Affinity.ToList(),
				System = new SystemModel
				{
					Os = new OsModel
					{
						Category = resultDto.System.Os.Category,
						Name = resultDto.System.Os.Name,
						Version = resultDto.System.Os.Version
					},
					Cpu =
						new CpuModel
						{
							Brand = resultDto.System.Cpu.Brand,
							Family = resultDto.System.Cpu.Family,
							Frequency = resultDto.System.Cpu.Frequency,
							Model = resultDto.System.Cpu.Model,
							Smt = resultDto.System.Cpu.Smt,
							Stepping = resultDto.System.Cpu.Stepping,
							Features = resultDto.System.Cpu.Features,
							Vendor = resultDto.System.Cpu.Vendor,
							TurboBoost = resultDto.System.Cpu.TurboBoost,
							TurboBoost3 = resultDto.System.Cpu.TurboBoost3,
							Caches = resultDto.System.Cpu.Caches.Select(c => new CpuCacheModel
							{
								Associativity = c.Associativity,
								Name = c.Name,
								Size = c.Size,
								LineSize = c.LineSize,
								LinesPerTag = c.LinesPerTag
							}).ToList()
						},
					Memory =
						new MemoryModel
						{
							PageSize = resultDto.System.Memory.PageSize,
							TotalSize = resultDto.System.Memory.TotalSize
						},
					Topology = new TopologyModel
					{
						TotalDepth = resultDto.System.Topology.TotalDepth,
						TotalLogicalCores = resultDto.System.Topology.TotalLogicalCores,
						TotalPhysicalCores = resultDto.System.Topology.TotalLogicalCores,
						Root = new CpuNodeModel
						{
							Name = resultDto.System.Topology.Root.Name,
							Value = resultDto.System.Topology.Root.Value,
							NodeType = resultDto.System.Topology.Root.NodeType,
							OsIndex = resultDto.System.Topology.Root.OsIndex
						}
					}
				}
			};

			if (resultDto.System.Topology.Root.Children != null)
			{
				resultModel.System.Topology.Root.Children = new List<CpuNodeModel>();
				foreach (var child in resultDto.System.Topology.Root.Children)
				{
					resultModel.System.Topology.Root.Children.Add(CreateChild(child));
				}
			}

			if (resultDto.System.Topology.Root.MemoryChildren != null)
			{
				resultModel.System.Topology.Root.MemoryChildren = new List<CpuNodeModel>();
				foreach (var child in resultDto.System.Topology.Root.MemoryChildren)
				{
					resultModel.System.Topology.Root.MemoryChildren.Add(CreateChild(child));
				}
			}


			return resultModel;
		}

		private static CpuNodeModel CreateChild(CpuNodeDto cpuNodeDto)
		{
			return new CpuNodeModel
			{
				Name = cpuNodeDto.Name,
				Value = cpuNodeDto.Value,
				NodeType = cpuNodeDto.NodeType,
				OsIndex = cpuNodeDto.OsIndex,
				Children = cpuNodeDto.Children?.Select(CreateChild).ToList(),
				MemoryChildren = cpuNodeDto.MemoryChildren?.Select(CreateChild).ToList()
			};
		}
	}
}