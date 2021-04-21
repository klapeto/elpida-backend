/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Result
{
    public static class TaskResultDataExtensions
    {
        public static TaskResultModel ToModel(this TaskResultDto taskResultDto)
        {
            return new TaskResultModel
            {
                Id = taskResultDto.Id,
                TaskId = taskResultDto.TaskId,
                BenchmarkResultId = taskResultDto.BenchmarkResultId,
                CpuId = taskResultDto.CpuId,
                TopologyId = taskResultDto.TopologyId,
                Time = taskResultDto.Time,
                Value = taskResultDto.Value,
                InputSize = taskResultDto.InputSize,
                Max = taskResultDto.Statistics.Max,
                Mean = taskResultDto.Statistics.Mean,
                Min = taskResultDto.Statistics.Min,
                StandardDeviation = taskResultDto.Statistics.Sd,
                Tau = taskResultDto.Statistics.Tau,
                SampleSize = taskResultDto.Statistics.SampleSize,
                MarginOfError = taskResultDto.Statistics.MarginOfError
            };
        }
    }
}