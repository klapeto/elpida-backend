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
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Result;

namespace Elpida.Backend.Services
{
    public class BenchmarkResultService : Service<ResultDto, BenchmarkResultModel>, IBenchmarkResultsService
    {
        private readonly IBenchmarkService _benchmarkService;
        private readonly ICpuService _cpuService;
        private readonly IElpidaService _elpidaService;
        private readonly IOsService _osService;
        private readonly IStatisticsService _statisticsService;
        private readonly ITaskService _taskService;
        private readonly ITopologyService _topologyService;

        private static IEnumerable<FilterExpression> ResultFilters { get; } = new List<FilterExpression>
        {
            CreateFilter("memorySize", model => model.MemorySize),
            CreateFilter("timeStamp", model => model.TimeStamp),
        };
        
        #region IResultsService Members

        public BenchmarkResultService(IBenchmarkService benchmarkService,
            ITopologyService topologyService,
            IElpidaService elpidaService,
            ITaskService taskService,
            IOsService osService,
            IBenchmarkResultsRepository benchmarkResultsRepository,
            ICpuService cpuService,
            IStatisticsService statisticsService)
            : base(benchmarkResultsRepository)
        {
            _benchmarkService = benchmarkService;
            _topologyService = topologyService;
            _elpidaService = elpidaService;
            _taskService = taskService;
            _osService = osService;
            _cpuService = cpuService;
            _statisticsService = statisticsService;
        }

        protected override async Task PreProcessDtoForCreationAsync(ResultDto dto, CancellationToken cancellationToken)
        {
            var benchmark = await _benchmarkService.GetSingleAsync(dto.Result.Uuid, cancellationToken);
            var cpu = await _cpuService.GetOrAddAsync(dto.System.Cpu, cancellationToken);
            var topology = await _topologyService.GetOrAddAsync(dto.System.Topology, cancellationToken);
            var elpida = await _elpidaService.GetOrAddAsync(dto.Elpida, cancellationToken);
            var os = await _osService.GetOrAddAsync(dto.System.Os, cancellationToken);

            dto.TimeStamp = DateTime.UtcNow;

            foreach (var taskResult in dto.Result.TaskResults)
            {
                var task = await _taskService.GetSingleAsync(taskResult.Uuid, cancellationToken);
                taskResult.Id = 0;
                taskResult.TaskId = task.Id;
                taskResult.TopologyId = topology.Id;
                taskResult.CpuId = cpu.Id;
            }

            dto.Elpida = elpida;
            dto.System.Cpu = cpu;
            dto.System.Topology = topology;
            dto.Result.Id = benchmark.Id;
            dto.System.Os = os;
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

        protected override BenchmarkResultModel ToModel(ResultDto dto)
        {
            return dto.ToModel();
        }

        protected override Task OnEntityCreatedAsync(ResultDto dto, BenchmarkResultModel entity,
            CancellationToken cancellationToken)
        {
            return _statisticsService.AddBenchmarkResultAsync(dto, cancellationToken);
        }

        #endregion
    }
}