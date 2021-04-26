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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Extensions.Task;
using Elpida.Backend.Services.Extensions.Topology;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public class TaskTaskStatisticsService :
        Service<TaskStatisticsDto, TaskStatisticsModel, ITaskStatisticsRepository>,
        ITaskStatisticsService
    {
        private readonly ICpuService _cpuService;
        private readonly ITaskService _taskService;
        private readonly ITopologyService _topologyService;

        public TaskTaskStatisticsService(ITaskService taskService,
            ITopologyService topologyService,
            ITaskStatisticsRepository taskStatisticsRepository,
            ICpuService cpuService)
            : base(taskStatisticsRepository)
        {
            _topologyService = topologyService;
            _cpuService = cpuService;
            _taskService = taskService;
        }

        public async Task UpdateTaskStatisticsAsync(IEnumerable<TaskResultDto> taskResults,
            CancellationToken cancellationToken = default)
        {
            foreach (var taskResult in taskResults)
            {
                var topology = await _topologyService.GetSingleAsync(taskResult.TopologyId, cancellationToken);
                var task = await _taskService.GetSingleAsync(taskResult.Uuid, cancellationToken);

                var stats = await Repository.GetSingleAsync(
                    t => t.TaskId == task.Id 
                         && t.TopologyId == topology.Id, cancellationToken);
                if (stats == null)
                {
                    stats = new TaskStatisticsModel
                    {
                        CpuId = topology.CpuId,
                        Max = taskResult.Value,
                        Mean = taskResult.Value,
                        Min = taskResult.Value,
                        TaskId = task.Id,
                        TopologyId = topology.Id,
                        SampleSize = 1,
                        TotalDeviation = 0,
                        TotalValue = taskResult.Value,
                        Tau = StatisticsHelpers.CalculateTau(1),
                        StandardDeviation = 0,
                        MarginOfError = 0
                    };
                    await Repository.CreateAsync(stats, cancellationToken);
                }
                else
                {
                    stats.Max = Math.Max(stats.Max, taskResult.Value);
                    stats.Min = Math.Min(stats.Min, taskResult.Value);
                    stats.TotalValue += taskResult.Value;
                    stats.SampleSize++;
                    stats.Mean = stats.TotalValue / stats.SampleSize;
                    stats.TotalDeviation += Math.Pow(taskResult.Value - stats.Mean, 2.0);
                    stats.StandardDeviation = Math.Sqrt(stats.TotalDeviation / stats.SampleSize);
                    stats.MarginOfError = stats.StandardDeviation / Math.Sqrt(stats.SampleSize);
                    stats.Tau = StatisticsHelpers.CalculateTau(stats.SampleSize);
                }
            }

            await Repository.SaveChangesAsync(cancellationToken);
        }

        public Task<PagedResult<TaskStatisticsPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionsAsync(queryRequest, m => new TaskStatisticsPreviewDto
            {
                Id = m.Id,
                CpuVendor = m.Cpu.Vendor,
                CpuBrand = m.Cpu.Brand,
                CpuCores = m.Topology.TotalPhysicalCores,
                CpuLogicalCores = m.Topology.TotalLogicalCores,
                TaskName = m.Task.Name,
                SampleSize = m.SampleSize,
                TopologyHash = m.Topology.TopologyHash,
                TaskResultUnit = m.Task.ResultUnit,
                Mean = m.Mean
            }, cancellationToken);
        }

        protected override Task<TaskStatisticsModel> ProcessDtoAndCreateModelAsync(TaskStatisticsDto dto, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TaskStatisticsModel
            {
                Id = dto.Id,
                CpuId = dto.Cpu.Id,
                TopologyId = dto.Topology.Id,
                TaskId = dto.Task.Id,
                Max = dto.Max,
                Mean = dto.Mean,
                Min = dto.Min,
                Tau = dto.Tau,
                SampleSize = dto.SampleSize,
                StandardDeviation = dto.StandardDeviation,
                MarginOfError = dto.MarginOfError
            });
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return _cpuService.GetFilters<TaskStatisticsModel, CpuModel>(m => m.Cpu)
                .Concat(_topologyService.GetFilters<TaskStatisticsModel, TopologyModel>(m => m.Topology))
                .Concat(_taskService.GetFilters<TaskStatisticsModel, TaskModel>(m => m.Task));
        }

        protected override TaskStatisticsDto ToDto(TaskStatisticsModel model)
        {
            return new TaskStatisticsDto
            {
                Id = model.Id,
                Cpu = model.Cpu.ToDto(),
                Task = model.Task.ToDto(),
                Topology = model.Topology.ToDto(),
                Max = model.Max,
                Mean = model.Mean,
                Min = model.Min,
                Tau = model.Tau,
                SampleSize = model.SampleSize,
                StandardDeviation = model.StandardDeviation,
                MarginOfError = model.MarginOfError
            };
        }
    }
}