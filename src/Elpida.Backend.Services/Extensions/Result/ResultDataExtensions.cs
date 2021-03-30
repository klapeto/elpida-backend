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

using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Extensions.Cpu;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class ResultDataExtensions
	{
		private static DataSpecificationDto? CreateInputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.InputName)) return null;

			return new DataSpecificationDto
			{
				Name = model.InputName,
				Description = model.InputDescription!,
				Unit = model.InputDescription!,
				RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.InputProperties!)
			};
		}

		private static DataSpecificationDto? CreateOutputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.OutputName)) return null;

			return new DataSpecificationDto
			{
				Name = model.OutputName,
				Description = model.OutputDescription!,
				Unit = model.OutputUnit!,
				RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.OutputProperties!)
			};
		}
		
		public static ResultDto ToDto(this ResultModel resultModel)
		{
			return new ResultDto
			{
				TimeStamp = resultModel.TimeStamp,
				Id = resultModel.Id,
				Elpida = new ElpidaDto
				{
					Compiler = new CompilerDto
					{
						Name = resultModel.CompilerName,
						Version = resultModel.CompilerVersion
					},
					Version = new VersionDto
					{
						Major = resultModel.ElpidaVersionMajor,
						Minor = resultModel.ElpidaVersionMinor,
						Revision = resultModel.ElpidaVersionRevision,
						Build = resultModel.ElpidaVersionBuild
					}
				},
				Affinity = JsonConvert.DeserializeObject<List<long>>(resultModel.Affinity),
				Result = new BenchmarkResultDto
				{
					Id = resultModel.Benchmark.Id,
					Uuid = resultModel.Benchmark.Uuid,
					Name = resultModel.Benchmark.Name,
					TaskResults = resultModel.TaskResults.Select(r => new TaskResultDto
					{
						Id = r.Task.Id,
						Uuid = r.Task.Uuid,
						Name = r.Task.Name,
						Description = r.Task.Description,
						Input = r.Task.CreateInputSpecDto(),
						Output = r.Task.CreateOutputSpecDto(),
						Result = new ResultSpecificationDto
						{
							Name = r.Task.ResultName,
							Description = r.Task.ResultDescription,
							Aggregation = r.Task.ResultAggregation,
							Type = r.Task.ResultType,
							Unit = r.Task.ResultUnit
						},
						Statistics = new TaskStatisticsDto
						{
							Max = r.Max,
							Mean = r.Mean,
							Min = r.Min,
							Sd = r.StandardDeviation,
							Tau = r.Tau,
							SampleSize = r.SampleSize,
							MarginOfError = r.MarginOfError
						},
						Time = r.Time,
						Value = r.Value,
						InputSize = r.InputSize
					}).ToList()
				},
				System = new SystemDto
				{
					Cpu = resultModel.Topology.Cpu.ToDto(),
					Memory = new MemoryDto
					{
						PageSize = resultModel.PageSize,
						TotalSize = resultModel.MemorySize
					},
					Os = new OsDto
					{
						Category = resultModel.OsCategory,
						Name = resultModel.OsName,
						Version = resultModel.OsVersion
					},
					Timing = new TimingDto
					{
						JoinOverhead = resultModel.JoinOverhead,
						LockOverhead = resultModel.LockOverhead,
						LoopOverhead = resultModel.LoopOverhead,
						NotifyOverhead = resultModel.NotifyOverhead,
						NowOverhead = resultModel.NowOverhead,
						SleepOverhead = resultModel.SleepOverhead,
						TargetTime = resultModel.TargetTime,
						WakeupOverhead = resultModel.WakeupOverhead
					},
					Topology = new TopologyDto
					{
						TotalDepth = resultModel.Topology.TotalDepth,
						TotalLogicalCores = resultModel.Topology.TotalLogicalCores,
						TotalPhysicalCores = resultModel.Topology.TotalPhysicalCores,
						Root = JsonConvert.DeserializeObject<CpuNodeDto>(resultModel.Topology.Root)
					}
				}
			};
		}

		public static ResultModel ToModel(this ResultDto resultDto,
			BenchmarkModel benchmarkModel,
			TopologyModel topologyModel,
			ICollection<TaskResultModel> resultModels)
		{
			return new ResultModel
			{
				Affinity = JsonConvert.SerializeObject(resultDto.Affinity),
				Benchmark = benchmarkModel,
				Topology = topologyModel,
				CompilerName = resultDto.Elpida.Compiler.Name,
				CompilerVersion = resultDto.Elpida.Compiler.Version,
				ElpidaVersionMajor = resultDto.Elpida.Version.Major,
				ElpidaVersionMinor = resultDto.Elpida.Version.Minor,
				ElpidaVersionRevision = resultDto.Elpida.Version.Revision,
				ElpidaVersionBuild = resultDto.Elpida.Version.Build,
				JoinOverhead = resultDto.System.Timing.JoinOverhead,
				LockOverhead = resultDto.System.Timing.LockOverhead,
				LoopOverhead = resultDto.System.Timing.LoopOverhead,
				NotifyOverhead = resultDto.System.Timing.NotifyOverhead,
				NowOverhead = resultDto.System.Timing.NowOverhead,
				SleepOverhead = resultDto.System.Timing.SleepOverhead,
				TargetTime = resultDto.System.Timing.TargetTime,
				WakeupOverhead = resultDto.System.Timing.WakeupOverhead,
				MemorySize = resultDto.System.Memory.TotalSize,
				PageSize = resultDto.System.Memory.PageSize,
				OsCategory = resultDto.System.Os.Category,
				OsName = resultDto.System.Os.Name,
				OsVersion = resultDto.System.Os.Version,
				TaskResults = resultModels,
				TimeStamp = resultDto.TimeStamp
			};
		}
	}
}