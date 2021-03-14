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
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Projections;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Extensions.Topology;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class ResultDataExtensions
	{
		public static ResultDto ToDto(this ResultProjection resultProjection)
		{
			return new ResultModel
			{
				Affinity = resultProjection.Affinity,
				Elpida = resultProjection.Elpida,
				Id = resultProjection.Id,
				Result = new BenchmarkResultModel
				{
					BenchmarkId = resultProjection.Result.Benchmark.Id,
					TaskResults = resultProjection.Result.TaskResults.Select(projection => new TaskResultModel
					{
						TaskId = projection.Task.Id,
						Outliers = projection.Result.Outliers,
						Statistics = projection.Result.Statistics,
						Time = projection.Result.Time,
						Value = projection.Result.Value,
						InputSize = projection.Result.InputSize
					}).ToList()
				},
				System = new SystemModel
				{
					Memory = resultProjection.System.Memory,
					Os = resultProjection.System.Os,
					Timing = resultProjection.System.Timing,
				},
				TimeStamp = resultProjection.TimeStamp
			}.ToDto(resultProjection.System.Cpu, 
				resultProjection.System.Topology, 
				resultProjection.Result.Benchmark,
				resultProjection.Result.TaskResults.Select(r => r.Task));
		}
		
		public static ResultDto ToDto(this ResultModel resultModel, 
			CpuModel cpuModel, 
			TopologyModel topologyModel,
			BenchmarkModel benchmarkModel,
			IEnumerable<TaskModel> tasks)
		{
			var resultDto = new ResultDto
			{
				TimeStamp = resultModel.TimeStamp,
				Id = resultModel.Id,
				Elpida = resultModel.Elpida.ToDto(),
				Affinity = resultModel.Affinity.ToList(),
				Result = resultModel.Result.ToDto(benchmarkModel, tasks),
				System = resultModel.System.ToDto(cpuModel, topologyModel)
			};

			return resultDto;
		}

		public static ResultModel ToModel(this ResultDto resultDto, string id, string cpuId, string topologyId)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("id cannot be empty", nameof(id));
			}

			if (string.IsNullOrWhiteSpace(cpuId))
			{
				throw new ArgumentException("cpuId cannot be empty", nameof(cpuId));
			}

			if (string.IsNullOrWhiteSpace(topologyId))
			{
				throw new ArgumentException("cpuId cannot be empty", nameof(topologyId));
			}

			return new ResultModel
			{
				Id = id,
				TimeStamp = resultDto.TimeStamp,
				Elpida = resultDto.Elpida.ToModel(),
				Result = resultDto.Result.ToModel(),
				Affinity = resultDto.Affinity.ToList(),
				System = resultDto.System.ToModel(cpuId, topologyId)
			};
		}
	}
}