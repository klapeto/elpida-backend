using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Tests
{
	public static class Generators
	{
		public static ResultPreviewModel CreateResultPreviewModel(string id)
		{
			return new ResultPreviewModel
			{
				Id = id,
				Name = "Test Benchmark",
				CpuBrand = "AMD",
				CpuCores = 12,
				CpuFrequency = 1654645646,
				MemorySize = 565565656,
				OsName = "Windows",
				OsVersion = "XP",
				TimeStamp = DateTime.UtcNow,
				CpuLogicalCores = 24,
				ElpidaVersionBuild = 6564,
				ElpidaVersionMajor = 1,
				ElpidaVersionMinor = 5,
				ElpidaVersionRevision = 6
			};
		}
		public static ResultModel CreateNewResultModel(string id)
		{
			return new ResultModel
			{
				Id = id,
				TimeStamp = DateTime.UtcNow,
				Affinity = new List<ulong> {5, 3, 1},
				Elpida = new ElpidaModel
				{
					Compiler = new CompilerModel
					{
						Name = "GCC",
						Version = "10.0.0"
					},
					Version = new VersionModel
					{
						Build = 2110,
						Major = 1,
						Minor = 0,
						Revision = 1
					}
				},
				Result = new BenchmarkResultModel
				{
					Name = "Memory Read Bandwidth",
					TaskResults = new List<TaskResultModel>
					{
						new TaskResultModel
						{
							Description = "Read Bandwidth",
							Name = "Read Bandwidth",
							Suffix = "B",
							Time = 255.0,
							Type = 0,
							Value = 656566.0,
							InputSize = 56665
						}
					}
				},
				System = new SystemModel
				{
					Cpu = new CpuModel
					{
						Brand = "AMD",
						Caches = new List<CpuCacheModel>
						{
							new CpuCacheModel
							{
								Associativity = "Full",
								Name = "L0",
								Size = 100000000000,
								LineSize = 64,
								LinesPerTag = 1
							}
						},
						Family = 0, Features = new List<string>(), Frequency = 45455555555, Model = 0, Smt = true,
						Stepping = 0, Vendor = "AMD", TurboBoost = false, TurboBoost3 = false
					},
					Memory = new MemoryModel
					{
						PageSize = 4096, TotalSize = 544465464
					},
					Os = new OsModel
					{
						Category = "Linux",
						Name = "KDE Neon",
						Version = "25.0"
					},
					Topology = new TopologyModel
					{
						Root = new CpuNodeModel
						{
							Children = new List<CpuNodeModel>
							{
								new CpuNodeModel
								{
									Children = new List<CpuNodeModel>(),
									Name = "Core",
									Value = null,
									MemoryChildren = new List<CpuNodeModel>(),
									NodeType = 1,
									OsIndex = 1
								}
							}, 
							Name = "Machine",
							Value = null, MemoryChildren = new List<CpuNodeModel>(),
							NodeType = 0,
							OsIndex = 0
						},
						TotalDepth = 1, TotalLogicalCores = 1, TotalPhysicalCores = 1
					}
				}
			};
		}
		
		
		public static ResultDto CreateNewResultDto()
		{
			return new ResultDto
			{
				Affinity = new List<ulong> {5, 3, 1},
				Elpida = new ElpidaDto
				{
					Compiler = new CompilerDto
					{
						Name = "GCC",
						Version = "10.0.0"
					},
					Version = new VersionDto
					{
						Build = 2110,
						Major = 1,
						Minor = 0,
						Revision = 1
					}
				},
				Result = new BenchmarkResultDto
				{
					Name = "Memory Read Bandwidth",
					TaskResults = new List<TaskResultDto>
					{
						new TaskResultDto
						{
							Description = "Read Bandwidth",
							Name = "Read Bandwidth",
							Suffix = "B",
							Time = 255.0,
							Type = 0,
							Value = 656566.0,
							InputSize = 56665
						}
					}
				},
				System = new SystemDto
				{
					Cpu = new CpuDto
					{
						Brand = "AMD",
						Caches = new List<CpuCacheDto>
						{
							new CpuCacheDto
							{
								Associativity = "Full",
								Name = "L0",
								Size = 100000000000,
								LineSize = 64,
								LinesPerTag = 1
							}
						},
						Family = 0, Features = new List<string>(), Frequency = 45455555555, Model = 0, Smt = true,
						Stepping = 0, Vendor = "AMD", TurboBoost = false, TurboBoost3 = false
					},
					Memory = new MemoryDto
					{
						PageSize = 4096, TotalSize = 544465464
					},
					Os = new OsDto
					{
						Category = "Linux",
						Name = "KDE Neon",
						Version = "25.0"
					},
					Topology = new TopologyDto
					{
						Root = new CpuNodeDto
						{
							Children = new List<CpuNodeDto>
							{
								new CpuNodeDto
								{
									Children = new List<CpuNodeDto>(),
									Name = "Core",
									Value = null,
									MemoryChildren = new List<CpuNodeDto>(),
									NodeType = 1,
									OsIndex = 1
								}
							}, 
							Name = "Machine",
							Value = null, MemoryChildren = new List<CpuNodeDto>(),
							NodeType = 0,
							OsIndex = 0
						},
						TotalDepth = 1, TotalLogicalCores = 1, TotalPhysicalCores = 1
					}
				}
			};
		}
	}
}