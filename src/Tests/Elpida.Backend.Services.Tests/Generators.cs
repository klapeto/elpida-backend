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
		public static AssetInfoModel CreateAssetInfoModel()
		{
			return new AssetInfoModel
			{
				Filename = "Test.xtx",
				Location = new Uri("https://beta.elpida.dev"),
				Md5 = "68546464654314",
				Size = 6546543132
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
						Features = new List<string>(), Frequency = 45455555555, Smt = true,
						Vendor = "AMD", AdditionalInfo = new Dictionary<string, string>()
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
							Value = null,
							MemoryChildren = new List<CpuNodeModel>
							{
								new CpuNodeModel
								{
									Children = new List<CpuNodeModel>(),
									Name = "NUMA",
									Value = 45654,
									MemoryChildren = new List<CpuNodeModel>(),
									NodeType = 7,
									OsIndex = 1
								},
								new CpuNodeModel
								{
									Children = new List<CpuNodeModel>(),
									Name = "NUMA2",
									Value = 4565455,
									MemoryChildren = new List<CpuNodeModel>(),
									NodeType = 7,
									OsIndex = 2
								}
							},
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
						Features = new List<string>(), Frequency = 45455555555, Smt = true,
						Vendor = "AMD", AdditionalInfo = new Dictionary<string, string>()
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
						TotalDepth = 1, TotalLogicalCores = 1, TotalPhysicalCores = 1
					}
				}
			};
		}
	}
}