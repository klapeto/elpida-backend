/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020 Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
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

        public static ResultDto ToDto(this BenchmarkResultModel benchmarkResultModel)
        {
            return new ResultDto
            {
                TimeStamp = benchmarkResultModel.TimeStamp,
                Id = benchmarkResultModel.Id,
                Elpida = new ElpidaDto
                {
                    Compiler = new CompilerDto
                    {
                        Name = benchmarkResultModel.Elpida.CompilerName,
                        Version = benchmarkResultModel.Elpida.CompilerVersion
                    },
                    Version = new VersionDto
                    {
                        Major = benchmarkResultModel.Elpida.VersionMajor,
                        Minor = benchmarkResultModel.Elpida.VersionMinor,
                        Revision = benchmarkResultModel.Elpida.VersionRevision,
                        Build = benchmarkResultModel.Elpida.VersionBuild
                    }
                },
                Affinity = JsonConvert.DeserializeObject<List<long>>(benchmarkResultModel.Affinity),
                Result = new BenchmarkResultDto
                {
                    Id = benchmarkResultModel.Benchmark.Id,
                    Uuid = benchmarkResultModel.Benchmark.Uuid,
                    Name = benchmarkResultModel.Benchmark.Name,
                    TaskResults = benchmarkResultModel.TaskResults
                        .OrderBy(m => m.Order)
                        .Select(r => new TaskResultDto
                    {
                        Id = r.Task.Id,
                        Uuid = r.Task.Uuid,
                        Name = r.Task.Name,
                        CpuId = benchmarkResultModel.Topology.Cpu.Id,
                        TopologyId = benchmarkResultModel.Topology.Id,
                        TaskId = r.Task.Id,
                        BenchmarkResultId = benchmarkResultModel.Id,
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
                        Statistics = new TaskRunStatisticsDto
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
                    Cpu = benchmarkResultModel.Topology.Cpu.ToDto(),
                    Memory = new MemoryDto
                    {
                        PageSize = benchmarkResultModel.PageSize,
                        TotalSize = benchmarkResultModel.MemorySize
                    },
                    Os = new OsDto
                    {
                        Category = benchmarkResultModel.Os.Category,
                        Name = benchmarkResultModel.Os.Name,
                        Version = benchmarkResultModel.Os.Version
                    },
                    Timing = new TimingDto
                    {
                        JoinOverhead = benchmarkResultModel.JoinOverhead,
                        LockOverhead = benchmarkResultModel.LockOverhead,
                        LoopOverhead = benchmarkResultModel.LoopOverhead,
                        NotifyOverhead = benchmarkResultModel.NotifyOverhead,
                        NowOverhead = benchmarkResultModel.NowOverhead,
                        SleepOverhead = benchmarkResultModel.SleepOverhead,
                        TargetTime = benchmarkResultModel.TargetTime,
                        WakeupOverhead = benchmarkResultModel.WakeupOverhead
                    },
                    Topology = new TopologyDto
                    {
                        TotalDepth = benchmarkResultModel.Topology.TotalDepth,
                        TotalLogicalCores = benchmarkResultModel.Topology.TotalLogicalCores,
                        TotalPhysicalCores = benchmarkResultModel.Topology.TotalPhysicalCores,
                        Root = JsonConvert.DeserializeObject<CpuNodeDto>(benchmarkResultModel.Topology.Root)
                    }
                }
            };
        }

        public static BenchmarkResultModel ToModel(this ResultDto resultDto)
        {
            return new BenchmarkResultModel
            {
                Affinity = JsonConvert.SerializeObject(resultDto.Affinity),
                BenchmarkId = resultDto.Result.Id,
                TopologyId = resultDto.System.Topology.Id,
                OsId = resultDto.System.Os.Id,
                ElpidaId = resultDto.Elpida.Id,
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
                TaskResults = resultDto.Result.TaskResults.Select(t => t.ToModel()).ToList(),
                TimeStamp = resultDto.TimeStamp
            };
        }
    }
}