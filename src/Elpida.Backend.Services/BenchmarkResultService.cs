﻿/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Result;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
    public class BenchmarkResultService : Service<ResultDto, BenchmarkResultModel, IBenchmarkResultsRepository>,
        IBenchmarkResultsService
    {
        private readonly IBenchmarkRepository _benchmarkRepository;
        private readonly IBenchmarkService _benchmarkService;
        private readonly ICpuRepository _cpuRepository;

        private readonly ICpuService _cpuService;
        private readonly IElpidaRepository _elpidaRepository;
        private readonly IElpidaService _elpidaService;
        private readonly IOsRepository _osRepository;
        private readonly IOsService _osService;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskStatisticsService _taskStatisticsService;
        private readonly ITopologyRepository _topologyRepository;
        private readonly ITopologyService _topologyService;

        private static IEnumerable<FilterExpression> ResultFilters { get; } = new List<FilterExpression>
        {
            CreateFilter("memorySize", model => model.MemorySize),
            CreateFilter("timeStamp", model => model.TimeStamp)
        };

        public Task<PagedResult<ResultPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionsAsync(queryRequest, m => new ResultPreviewDto
            {
                Id = m.Id,
                TimeStamp = m.TimeStamp,
                Name = m.Benchmark.Name,
                CpuBrand = m.Topology.Cpu.Brand,
                CpuCores = m.Topology.TotalPhysicalCores,
                CpuLogicalCores = m.Topology.TotalLogicalCores,
                CpuFrequency = m.Topology.Cpu.Frequency,
                MemorySize = m.MemorySize,
                OsName = m.Os.Name,
                OsVersion = m.Os.Version,
                ElpidaVersionBuild = m.Elpida.VersionBuild,
                ElpidaVersionMajor = m.Elpida.VersionMajor,
                ElpidaVersionMinor = m.Elpida.VersionMinor,
                ElpidaVersionRevision = m.Elpida.VersionRevision
            }, cancellationToken);
        }

        #region IResultsService Members

        public BenchmarkResultService(
            IBenchmarkResultsRepository benchmarkResultsRepository,
            ITaskStatisticsService taskStatisticsService,
            ICpuRepository cpuRepository,
            IBenchmarkRepository benchmarkRepository,
            ITopologyRepository topologyRepository,
            IElpidaRepository elpidaRepository,
            IOsRepository osRepository,
            ITaskRepository taskRepository,
            ICpuService cpuService,
            ITopologyService topologyService,
            IElpidaService elpidaService,
            IOsService osService,
            IBenchmarkService benchmarkService)
            : base(benchmarkResultsRepository)
        {
            _taskStatisticsService = taskStatisticsService;
            _cpuRepository = cpuRepository;
            _benchmarkRepository = benchmarkRepository;
            _topologyRepository = topologyRepository;
            _elpidaRepository = elpidaRepository;
            _osRepository = osRepository;
            _taskRepository = taskRepository;
            _cpuService = cpuService;
            _topologyService = topologyService;
            _elpidaService = elpidaService;
            _osService = osService;
            _benchmarkService = benchmarkService;
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return ResultFilters
                .Concat(_cpuService.GetFilters<BenchmarkResultModel, CpuModel>(m => m.Topology.Cpu))
                .Concat(_topologyService.GetFilters<BenchmarkResultModel, TopologyModel>(m => m.Topology))
                .Concat(_elpidaService.GetFilters<BenchmarkResultModel, ElpidaModel>(m => m.Elpida))
                .Concat(_osService.GetFilters<BenchmarkResultModel, OsModel>(m => m.Os))
                .Concat(_benchmarkService.GetFilters<BenchmarkResultModel, BenchmarkModel>(m => m.Benchmark));
        }

        protected override ResultDto ToDto(BenchmarkResultModel model)
        {
            return model.ToDto();
        }

        protected override async Task<BenchmarkResultModel> ProcessDtoAndCreateModelAsync(ResultDto dto,
            CancellationToken cancellationToken)
        {
            var benchmark = await _benchmarkRepository
                .GetSingleAsync(b => b.Uuid == dto.Result.Uuid, cancellationToken);
            if (benchmark == null) throw new NotFoundException("Benchmark was not found", dto.Result.Uuid);

            var cpu = await GetOrAddForeignDto(_cpuRepository, _cpuService, dto.System.Cpu, cancellationToken);

            dto.System.Topology.CpuId = cpu.Id;
            var topology = await GetOrAddForeignDto(_topologyRepository, _topologyService, dto.System.Topology,
                cancellationToken);
            var elpida = await GetOrAddForeignDto(_elpidaRepository, _elpidaService, dto.Elpida, cancellationToken);
            var os = await GetOrAddForeignDto(_osRepository, _osService, dto.System.Os, cancellationToken);

            var model = new BenchmarkResultModel
            {
                Benchmark = benchmark,
                Elpida = elpida,
                Topology = topology,
                Os = os,
                Affinity = JsonConvert.SerializeObject(dto.Affinity),
                JoinOverhead = dto.System.Timing.JoinOverhead,
                LockOverhead = dto.System.Timing.LockOverhead,
                LoopOverhead = dto.System.Timing.LoopOverhead,
                NotifyOverhead = dto.System.Timing.NotifyOverhead,
                NowOverhead = dto.System.Timing.NowOverhead,
                SleepOverhead = dto.System.Timing.SleepOverhead,
                TargetTime = dto.System.Timing.TargetTime,
                WakeupOverhead = dto.System.Timing.WakeupOverhead,
                MemorySize = dto.System.Memory.TotalSize,
                PageSize = dto.System.Memory.PageSize,
                TaskResults = new List<TaskResultModel>(),
                TimeStamp = DateTime.UtcNow
            };

            var order = 0;
            foreach (var taskResult in dto.Result.TaskResults)
            {
                var task = await _taskRepository.GetSingleAsync(t => t.Uuid == taskResult.Uuid, cancellationToken);
                if (task == null) throw new NotFoundException("Task was not found", taskResult.Uuid);
                model.TaskResults.Add(new TaskResultModel
                {
                    Cpu = cpu,
                    Task = task,
                    Topology = topology,
                    BenchmarkResult = model,
                    Max = taskResult.Statistics.Max,
                    Mean = taskResult.Statistics.Mean,
                    Min = taskResult.Statistics.Min,
                    Tau = taskResult.Statistics.Tau,
                    SampleSize = taskResult.Statistics.SampleSize,
                    StandardDeviation = taskResult.Statistics.Sd,
                    MarginOfError = taskResult.Statistics.MarginOfError,
                    InputSize = taskResult.InputSize,
                    Value = taskResult.Value,
                    Time = taskResult.Time,
                    Order = order++
                });
            }

            return model;
        }

        protected override Task OnEntityCreatedAsync(
            ResultDto dto,
            BenchmarkResultModel entity,
            CancellationToken cancellationToken)
        {
            return _taskStatisticsService.UpdateTaskStatisticsAsync(dto.Result.TaskResults, cancellationToken);
        }

        #endregion
    }
}