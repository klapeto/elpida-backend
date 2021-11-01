// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

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
		public static BenchmarkResultDto ToDto(this BenchmarkResultModel benchmarkResultModel)
		{
			var scoreSpec = new BenchmarkScoreSpecificationDto
			{
				Unit = benchmarkResultModel.Benchmark.ScoreUnit,
				Comparison = benchmarkResultModel.Benchmark.ScoreComparison,
			};

			return new BenchmarkResultDto
			{
				Id = benchmarkResultModel.Id,
				TimeStamp = benchmarkResultModel.TimeStamp,
				Uuid = benchmarkResultModel.Benchmark.Uuid,
				Name = benchmarkResultModel.Benchmark.Name,
				Affinity = JsonConvert.DeserializeObject<long[]>(benchmarkResultModel.Affinity)!,
				ElpidaVersion = benchmarkResultModel.ElpidaVersion.ToDto(),
				System = GetSystem(benchmarkResultModel),
				Score = benchmarkResultModel.Score,
				ScoreSpecification = scoreSpec,
				TaskResults = GetTaskResults(benchmarkResultModel).ToArray(),
			};
		}

		public static ResultSpecificationDto GetResultSpecificationDto(this TaskModel model)
		{
			return new ()
			{
				Name = model.ResultName,
				Description = model.ResultDescription,
				Unit = model.ResultUnit,
				Aggregation = model.ResultAggregation,
				Type = model.ResultType,
			};
		}

		public static DataSpecificationDto? CreateInputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.InputName))
			{
				return null;
			}

			return new DataSpecificationDto
			{
				Name = model.InputName,
				Description = model.InputDescription!,
				Unit = model.InputUnit!,
				RequiredProperties = JsonConvert.DeserializeObject<string[]>(model.InputProperties!)!,
			};
		}

		public static DataSpecificationDto? CreateOutputSpecDto(this TaskModel model)
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
				RequiredProperties = JsonConvert.DeserializeObject<string[]>(model.OutputProperties!)!,
			};
		}

		private static TaskRunStatisticsDto GetTaskRunStatisticsDto(TaskResultModel model)
		{
			return new ()
			{
				SampleSize = model.SampleSize,
				Max = model.Max,
				Min = model.Min,
				Mean = model.Mean,
				StandardDeviation = model.StandardDeviation,
				Tau = model.Tau,
				MarginOfError = model.MarginOfError,
			};
		}

		private static IEnumerable<TaskResultDto> GetTaskResults(BenchmarkResultModel result)
		{
			return result.TaskResults
				.OrderBy(m => m.Order)
				.Select(
					r => new TaskResultDto
					{
						Id = r.Task.Id,
						BenchmarkResultId = result.Id,
						CpuId = result.Topology.Cpu.Id,
						TopologyId = result.Topology.Id,
						Uuid = r.Task.Uuid,
						Name = r.Task.Name,
						Description = r.Task.Description,
						Result = GetResultSpecificationDto(r.Task),
						Input = r.Task.CreateInputSpecDto(),
						Output = r.Task.CreateOutputSpecDto(),
						Value = r.Value,
						Time = r.Time,
						InputSize = r.InputSize,
						Statistics = GetTaskRunStatisticsDto(r),
					}
				);
		}

		private static SystemDto GetSystem(BenchmarkResultModel result)
		{
			var memory = new MemoryDto
			{
				TotalSize = result.MemorySize,
				PageSize = result.PageSize,
			};

			var timing = new TimingDto
			{
				NotifyOverhead = result.NotifyOverhead,
				WakeupOverhead = result.WakeupOverhead,
				SleepOverhead = result.SleepOverhead,
				NowOverhead = result.NowOverhead,
				LockOverhead = result.LockOverhead,
				LoopOverhead = result.LoopOverhead,
				JoinOverhead = result.JoinOverhead,
				TargetTime = result.TargetTime,
			};

			return new SystemDto
			{
				Cpu = result.Topology.Cpu.ToDto(),
				Os = result.Os.ToDto(),
				Topology = result.Topology.ToDto(),
				Memory = memory,
				Timing = timing,
			};
		}
	}
}