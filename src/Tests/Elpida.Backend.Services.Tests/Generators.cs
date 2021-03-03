/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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

using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Tests
{
	public static class Generators
	{
		public static ResultProjection CreateProjection(string id)
		{
			var result = CreateNewResultModel(id);
			return new ResultProjection
			{
				Affinity = result.Affinity,
				Elpida = result.Elpida,
				Id = result.Id,
				Result = result.Result,
				System = new SystemModelProjection
				{
					Cpu = CreateNewCpuModel(),
					Memory = result.System.Memory,
					Os = result.System.Os,
					Topology = CreateNewTopology(),
					Timing = new TimingModel
					{
						JoinOverhead = 454,
						LockOverhead = 4664,
						LoopOverhead = 56465,
						NotifyOverhead = 6544,
						NowOverhead = 564456,
						SleepOverhead = 13213,
						TargetTime = 156341,
						WakeupOverhead = 155,
					}
				},
				TimeStamp = result.TimeStamp
			};
		}


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
				Affinity = new List<long> {5, 3, 1},
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
							InputSize = 56665,
							Outliers = new List<TaskOutlierModel>
							{
								new TaskOutlierModel
								{
									Time = 213,
									Value = 16341,
								},
								new TaskOutlierModel
								{
									Time = 65466,
									Value = 415,
								}
							},
							Statistics = new TaskStatisticsModel
							{
								Max = 141635,
								Mean = 52452,
								Min = 5465,
								Sd = 5464,
								Tau = 465,
								SampleSize = 54454,
								MarginOfError = 4441
							}
						}
					}
				},
				System = new SystemModel
				{
					CpuId = "AMD_TR",
					TopologyId = "NUMA_C",
					Timing = new TimingModel
					{
						JoinOverhead = 0.23,
						LockOverhead = 021,
						LoopOverhead = 3213,
						NotifyOverhead = 12365,
						NowOverhead = 6132,
						SleepOverhead = 1566,
						TargetTime = 4684,
						WakeupOverhead = 5644
					},
					Memory = new MemoryModel
					{
						PageSize = 4096,
						TotalSize = 544465464
					},
					Os = new OsModel
					{
						Category = "Linux",
						Name = "KDE Neon",
						Version = "25.0"
					}
				}
			};
		}

		public static CpuModel CreateNewCpuModel()
		{
			return new CpuModel
			{
				Brand = "Sdsf",
				Caches = new List<CpuCacheModel>
				{
					new CpuCacheModel
						{Associativity = "Full", Name = "L1", Size = 4546, LineSize = 65465, LinesPerTag = 46462}
				},
				Features = new List<string> {"LOL", "XD"},
				Frequency = 644,
				Id = "sadfsdfsd",
				Smt = false,
				Vendor = "dsafdsf",
				AdditionalInfo = new Dictionary<string, string> {["Fast"] = "yes", ["Sffv"] = "dfsf"}
			};
		}

		public static TopologyModel CreateNewTopology()
		{
			return new TopologyModel
			{
				Id = "dsfds",
				Root = new CpuNodeModel
				{
					Name = "dsfdsf",
					Children = null,
					Value = 455,
					MemoryChildren = null,
					NodeType = 1,
					OsIndex = 0
				},
				TotalDepth = 1,
				TotalLogicalCores = 1,
				TotalPhysicalCores = 1
			};
		}

		public static ResultDto CreateNewResultDto()
		{
			return new ResultDto
			{
				Id = Guid.NewGuid().ToString("N"),
				Affinity = new List<long> {5, 3, 1},
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
							InputSize = 56665,
							Outliers = new List<TaskOutlierDto>
							{
								new TaskOutlierDto
								{
									Time = 0.5554,
									Value = 163163,
								},
								new TaskOutlierDto
								{
									Time = 0.546896,
									Value = 654464,
								}
							},
							Statistics = new TaskStatisticsDto
							{
								Max = 15165,
								Mean = 6454,
								Min = 541541,
								Sd = 145,
								Tau = 154465,
								SampleSize = 4654456,
								MarginOfError = 211564,
							}
						}
					}
				},
				System = new SystemDto
				{
					Timing = new TimingDto
					{
						JoinOverhead = 0.23,
						LockOverhead = 021,
						LoopOverhead = 3213,
						NotifyOverhead = 12365,
						NowOverhead = 6132,
						SleepOverhead = 1566,
						TargetTime = 4684,
						WakeupOverhead = 5644
					},
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
						Features = new List<string>(),
						Frequency = 45455555555,
						Smt = true,
						Vendor = "AMD",
						AdditionalInfo = new Dictionary<string, string>()
					},
					Memory = new MemoryDto
					{
						PageSize = 4096,
						TotalSize = 544465464
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
							MemoryChildren = new List<CpuNodeDto>
							{
								new CpuNodeDto
								{
									Children = new List<CpuNodeDto>(),
									Name = "NUMA",
									Value = 45654,
									MemoryChildren = new List<CpuNodeDto>(),
									NodeType = 7,
									OsIndex = 1
								},
								new CpuNodeDto
								{
									Children = new List<CpuNodeDto>(),
									Name = "NUMA2",
									Value = 4565455,
									MemoryChildren = new List<CpuNodeDto>(),
									NodeType = 7,
									OsIndex = 2
								}
							},
							Name = "Machine",
							Value = null,
							NodeType = 0,
							OsIndex = 0
						},
						TotalDepth = 1,
						TotalLogicalCores = 1,
						TotalPhysicalCores = 1
					}
				}
			};
		}
	}
}