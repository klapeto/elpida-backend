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
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Task;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Extensions.Elpida;
using Elpida.Backend.Services.Extensions.Os;
using Elpida.Backend.Services.Extensions.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class ResultDataExtensions
	{
		public static ResultDto ToDto(this BenchmarkResultModel benchmarkResultModel)
		{
			return new ()
			{
				TimeStamp = benchmarkResultModel.TimeStamp,
				Id = benchmarkResultModel.Id,
				Elpida = benchmarkResultModel.Elpida.ToDto(),
				Affinity = JsonConvert.DeserializeObject<List<long>>(benchmarkResultModel.Affinity),
				Result = new BenchmarkResultDto
				{
					Id = benchmarkResultModel.Benchmark.Id,
					Uuid = benchmarkResultModel.Benchmark.Uuid,
					Name = benchmarkResultModel.Benchmark.Name,
					ScoreSpecification = new BenchmarkScoreSpecificationDto
					{
						Unit = benchmarkResultModel.Benchmark.ScoreUnit,
						Comparison = benchmarkResultModel.Benchmark.ScoreComparison,
					},
					Score = benchmarkResultModel.Score,
					TaskResults = benchmarkResultModel.TaskResults
						.OrderBy(m => m.Order)
						.Select(
							r => new TaskResultDto
							{
								Id = r.Task.Id,
								Uuid = r.Task.Uuid,
								Name = r.Task.Name,
								CpuId = benchmarkResultModel.Cpu.Id,
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
									Aggregation = (AggregationType)r.Task.ResultAggregation,
									Type = (ResultType)r.Task.ResultType,
									Unit = r.Task.ResultUnit,
								},
								Statistics = new TaskRunStatisticsDto
								{
									Max = r.Max,
									Mean = r.Mean,
									Min = r.Min,
									Sd = r.StandardDeviation,
									Tau = r.Tau,
									SampleSize = r.SampleSize,
									MarginOfError = r.MarginOfError,
								},
								Time = r.Time,
								Value = r.Value,
								InputSize = r.InputSize,
							}
						)
						.ToList(),
				},
				System = new SystemDto
				{
					Cpu = benchmarkResultModel.Cpu.ToDto(),
					Os = benchmarkResultModel.Os.ToDto(),
					Memory = new MemoryDto
					{
						PageSize = benchmarkResultModel.PageSize,
						TotalSize = benchmarkResultModel.MemorySize,
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
						WakeupOverhead = benchmarkResultModel.WakeupOverhead,
					},
					Topology = benchmarkResultModel.Topology.ToDto(),
				},
			};
		}

		private static DataSpecificationDto? CreateInputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.InputName))
			{
				return null;
			}

			return new DataSpecificationDto
			{
				Name = model.InputName,
				Description = model.InputDescription!,
				Unit = model.InputDescription!,
				RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.InputProperties!),
			};
		}

		private static DataSpecificationDto? CreateOutputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.OutputName))
			{
				return null;
			}

			return new DataSpecificationDto
			{
				Name = model.OutputName,
				Description = model.OutputDescription!,
				Unit = model.OutputUnit!,
				RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.OutputProperties!),
			};
		}
	}
}