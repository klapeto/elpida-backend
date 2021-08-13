// /*
//  * Elpida HTTP Rest API
//  *   
//  * Copyright (C) 2020  Ioannis Panagiotopoulos
//  *
//  * This program is free software: you can redistribute it and/or modify
//  * it under the terms of the GNU Affero General Public License as
//  * published by the Free Software Foundation, either version 3 of the
//  * License, or (at your option) any later version.
//  *
//  * This program is distributed in the hope that it will be useful,
//  * but WITHOUT ANY WARRANTY; without even the implied warranty of
//  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  * GNU Affero General Public License for more details.
//  *
//  * You should have received a copy of the GNU Affero General Public License
//  * along with this program.  If not, see <https://www.gnu.org/licenses/>.
//  */
//
// using System;
// using System.Collections.Generic;
// using Elpida.Backend.Data.Abstractions.Models.Benchmark;
// using Elpida.Backend.Data.Abstractions.Models.Cpu;
// using Elpida.Backend.Data.Abstractions.Models.Result;
// using Elpida.Backend.Data.Abstractions.Models.Task;
// using Elpida.Backend.Data.Abstractions.Models.Topology;
// using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
// using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
// using Elpida.Backend.Services.Abstractions.Dtos.Os;
// using Elpida.Backend.Services.Abstractions.Dtos.Result;
// using Elpida.Backend.Services.Abstractions.Dtos.Task;
// using Elpida.Backend.Services.Abstractions.Dtos.Topology;
//
// namespace Elpida.Backend.Services.Tests
// {
// 	public static class Generators
// 	{
// 		public static TaskModel CreateNewTaskModel()
// 		{
// 			return new()
// 			{
// 				Id = 131,
// 				Description = "sdfsdf",
// 				Name = "dsfdsfds",
// 				Uuid = Guid.NewGuid(),
// 				InputDescription = "sdfsdf",
// 				InputName = "sdfsdfdsf",
// 				InputProperties = "[]",
// 				InputUnit = "s",
// 				OutputDescription = "sadfsdfsd",
// 				OutputName = "sdfsdfdsf",
// 				OutputProperties = "[]",
// 				OutputUnit = "s",
// 				ResultAggregation = 2,
// 				ResultDescription = "dsfsdfs",
// 				ResultName = "sdfsdfsd",
// 				ResultType = 211,
// 				ResultUnit = "LOLs",
// 			};
// 		}
//
// 		public static ResultPreviewModel CreateResultPreviewModel(long id)
// 		{
// 			return new ResultPreviewModel
// 			{
// 				Id = id,
// 				Name = "Test Benchmark",
// 				CpuBrand = "AMD",
// 				CpuCores = 12,
// 				CpuFrequency = 1654645646,
// 				MemorySize = 565565656,
// 				OsName = "Windows",
// 				OsVersion = "XP",
// 				TimeStamp = DateTime.UtcNow,
// 				CpuLogicalCores = 24,
// 				ElpidaVersionBuild = 6564,
// 				ElpidaVersionMajor = 1,
// 				ElpidaVersionMinor = 5,
// 				ElpidaVersionRevision = 6,
// 			};
// 		}
//
// 		public static BenchmarkResultModel CreateNewResultModel(long id)
// 		{
// 			return new()
// 			{
// 				Id = id,
// 				TimeStamp = DateTime.UtcNow,
// 				Affinity = "[1,2,3]",
// 				Benchmark = new BenchmarkModel
// 				{
// 					Id = 123,
// 					Name = "Memeory Read",
// 					Uuid = Guid.NewGuid(),
// 				},
// 				Topology = CreateNewTopology(),
// 				ElpidaVersionBuild = 131,
// 				ElpidaVersionMajor = 153,
// 				ElpidaVersionMinor = 1564,
// 				ElpidaVersionRevision = 1354,
// 				CompilerName = "GCC",
// 				CompilerVersion = "10.0.0",
// 				JoinOverhead = 0.23,
// 				LockOverhead = 021,
// 				LoopOverhead = 3213,
// 				NotifyOverhead = 12365,
// 				NowOverhead = 6132,
// 				SleepOverhead = 1566,
// 				TargetTime = 4684,
// 				WakeupOverhead = 5644,
// 				MemorySize = 616584,
// 				OsCategory = "Linux",
// 				OsName = "Windows",
// 				OsVersion = "1.12",
// 				PageSize = 32513,
// 				TaskResults = new List<TaskResultModel>
// 				{
// 					new()
// 					{
// 						Id = 464,
// 						Time = 255.0,
// 						Value = 656566.0,
// 						InputSize = 56665,
// 						Max = 141635,
// 						Mean = 52452,
// 						Min = 5465,
// 						StandardDeviation = 5464,
// 						Tau = 465,
// 						SampleSize = 54454,
// 						MarginOfError = 4441,
// 						Task = CreateNewTaskModel(),
// 					},
// 				},
// 			};
// 		}
//
// 		public static CpuModel CreateNewCpuModel()
// 		{
// 			return new()
// 			{
// 				ModelName = "Sdsf",
// 				Caches = "[]",
// 				Features = "[]",
// 				Frequency = 644,
// 				Id = 32465,
// 				Smt = false,
// 				Vendor = "dsafdsf",
// 				AdditionalInfo = "{}",
// 			};
// 		}
//
// 		public static TopologyModel CreateNewTopology()
// 		{
// 			return new()
// 			{
// 				Id = 4654,
// 				Root = "null",
// 				TotalDepth = 1,
// 				TotalLogicalCores = 1,
// 				TotalPhysicalCores = 1,
// 				Cpu = CreateNewCpuModel(),
// 				TopologyHash = "sdfgsdfsdf",
// 			};
// 		}
//
// 		public static ResultDto CreateNewResultDto()
// 		{
// 			return new()
// 			{
// 				Id = 4556,
// 				Affinity = new List<long> { 5, 3, 1 },
// 				Elpida = new ElpidaDto
// 				{
// 					Compiler = new CompilerDto
// 					{
// 						Name = "GCC",
// 						Version = "10.0.0",
// 					},
// 					Version = new VersionDto
// 					{
// 						Build = 2110,
// 						Major = 1,
// 						Minor = 0,
// 						Revision = 1,
// 					},
// 				},
// 				Result = new BenchmarkResultDto
// 				{
// 					Name = "Memory Read Bandwidth",
// 					TaskResults = new List<TaskResultDto>
// 					{
// 						new()
// 						{
// 							Description = "Read Bandwidth",
// 							Name = "Read Bandwidth",
// 							Id = 12,
// 							Input = new DataSpecificationDto
// 							{
// 								Name = "Ll;sf",
// 								Description = "dsfdsf",
// 								Unit = "lols",
// 								RequiredProperties = new List<string> { "lo" },
// 							},
// 							Output = new DataSpecificationDto
// 							{
// 								Name = "Lfsdfl;sf",
// 								Description = "dsdfssfdsf",
// 								Unit = "losdfsls",
// 								RequiredProperties = new List<string> { "lo", "sdfsdf" },
// 							},
// 							Result = new ResultSpecificationDto
// 							{
// 								Aggregation = 21,
// 								Description = "sadfsdfdsfsd",
// 								Name = "sdfsdfsf",
// 								Type = 123,
// 								Unit = "lsdfgvb",
// 							},
// 							Uuid = Guid.NewGuid(),
// 							Time = 255.0,
// 							Value = 656566.0,
// 							InputSize = 56665,
// 							Statistics = new TaskRunStatisticsDto
// 							{
// 								Max = 15165,
// 								Mean = 6454,
// 								Min = 541541,
// 								Sd = 145,
// 								Tau = 154465,
// 								SampleSize = 4654456,
// 								MarginOfError = 211564,
// 							},
// 						},
// 					},
// 				},
// 				System = new SystemDto
// 				{
// 					Timing = new TimingDto
// 					{
// 						JoinOverhead = 0.23,
// 						LockOverhead = 021,
// 						LoopOverhead = 3213,
// 						NotifyOverhead = 12365,
// 						NowOverhead = 6132,
// 						SleepOverhead = 1566,
// 						TargetTime = 4684,
// 						WakeupOverhead = 5644,
// 					},
// 					Cpu = new CpuDto
// 					{
// 						ModelName = "AMD",
// 						Caches = new List<CpuCacheDto>
// 						{
// 							new()
// 							{
// 								Associativity = "Full",
// 								Name = "L0",
// 								Size = 100000000000,
// 								LineSize = 64,
// 							},
// 						},
// 						Features = new List<string>(),
// 						Frequency = 45455555555,
// 						Smt = true,
// 						Vendor = "AMD",
// 						AdditionalInfo = new Dictionary<string, string>(),
// 					},
// 					Memory = new MemoryDto
// 					{
// 						PageSize = 4096,
// 						TotalSize = 544465464,
// 					},
// 					Os = new OsDto
// 					{
// 						Category = "Linux",
// 						Name = "KDE Neon",
// 						Version = "25.0",
// 					},
// 					Topology = new TopologyDto
// 					{
// 						Root = new CpuNodeDto
// 						{
// 							Children = new List<CpuNodeDto>
// 							{
// 								new()
// 								{
// 									Children = new List<CpuNodeDto>(),
// 									Name = "Core",
// 									Value = null,
// 									MemoryChildren = new List<CpuNodeDto>(),
// 									NodeType = 1,
// 									OsIndex = 1,
// 								},
// 							},
// 							MemoryChildren = new List<CpuNodeDto>
// 							{
// 								new()
// 								{
// 									Children = new List<CpuNodeDto>(),
// 									Name = "NUMA",
// 									Value = 45654,
// 									MemoryChildren = new List<CpuNodeDto>(),
// 									NodeType = 7,
// 									OsIndex = 1,
// 								},
// 								new()
// 								{
// 									Children = new List<CpuNodeDto>(),
// 									Name = "NUMA2",
// 									Value = 4565455,
// 									MemoryChildren = new List<CpuNodeDto>(),
// 									NodeType = 7,
// 									OsIndex = 2,
// 								},
// 							},
// 							Name = "Machine",
// 							Value = null,
// 							NodeType = 0,
// 							OsIndex = 0,
// 						},
// 						TotalDepth = 1,
// 						TotalLogicalCores = 1,
// 						TotalPhysicalCores = 1,
// 					},
// 				},
// 			};
// 		}
// 	}
// }