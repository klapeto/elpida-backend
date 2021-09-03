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
using Elpida.Backend.Common;
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
			var scoreSpec = new BenchmarkScoreSpecificationDto(
				benchmarkResultModel.Benchmark.ScoreUnit,
				benchmarkResultModel.Benchmark.ScoreComparison
			);

			return new BenchmarkResultDto(
				benchmarkResultModel.Id,
				benchmarkResultModel.TimeStamp,
				benchmarkResultModel.Benchmark.Uuid,
				benchmarkResultModel.Benchmark.Name,
				JsonConvert.DeserializeObject<long[]>(benchmarkResultModel.Affinity),
				benchmarkResultModel.Elpida.ToDto(),
				GetSystem(benchmarkResultModel),
				benchmarkResultModel.Score,
				scoreSpec,
				GetTaskResults(benchmarkResultModel).ToArray()
			);
		}

		public static ResultSpecificationDto GetResultSpecificationDto(this TaskModel model)
		{
			return new (
				model.ResultName,
				model.ResultDescription,
				model.ResultUnit,
				(AggregationType)model.ResultAggregation,
				(ResultType)model.ResultType
			);
		}

		public static DataSpecificationDto? CreateInputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.InputName))
			{
				return null;
			}

			return new DataSpecificationDto(
				model.InputName,
				model.InputDescription!,
				model.InputUnit!,
				JsonConvert.DeserializeObject<string[]>(model.InputProperties!)
			);
		}

		public static DataSpecificationDto? CreateOutputSpecDto(this TaskModel model)
		{
			if (string.IsNullOrWhiteSpace(model.OutputName))
			{
				return null;
			}

			return new DataSpecificationDto(
				model.OutputName,
				model.OutputDescription!,
				model.OutputUnit!,
				JsonConvert.DeserializeObject<string[]>(model.OutputProperties!)
			);
		}

		private static TaskRunStatisticsDto GetTaskRunStatisticsDto(TaskResultModel model)
		{
			return new (
				model.SampleSize,
				model.Max,
				model.Min,
				model.Mean,
				model.StandardDeviation,
				model.Tau,
				model.MarginOfError
			);
		}

		private static IEnumerable<TaskResultDto> GetTaskResults(BenchmarkResultModel result)
		{
			return result.TaskResults
				.OrderBy(m => m.Order)
				.Select(
					r => new TaskResultDto(
						r.Task.Id,
						result.Id,
						result.Cpu.Id,
						result.Topology.Id,
						r.Task.Uuid,
						r.Task.Name,
						r.Task.Description,
						GetResultSpecificationDto(r.Task),
						r.Task.CreateInputSpecDto(),
						r.Task.CreateOutputSpecDto(),
						r.Value,
						r.Time,
						r.InputSize,
						GetTaskRunStatisticsDto(r)
					)
				);
		}

		private static SystemDto GetSystem(BenchmarkResultModel result)
		{
			var memory = new MemoryDto(result.MemorySize, result.PageSize);
			var timing = new TimingDto(
				result.NotifyOverhead,
				result.WakeupOverhead,
				result.SleepOverhead,
				result.NowOverhead,
				result.LockOverhead,
				result.LoopOverhead,
				result.JoinOverhead,
				result.TargetTime
			);

			return new SystemDto(result.Cpu.ToDto(), result.Os.ToDto(), result.Topology.ToDto(), memory, timing);
		}
	}
}