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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Result;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public class BenchmarkResultService : IBenchmarkResultsService
    {
        private readonly IBenchmarkResultsRepository _benchmarkResultsRepository;
        private readonly ICpuService _cpuService;
        private readonly IBenchmarkService _benchmarkService;
        private readonly IElpidaService _elpidaService;
        private readonly IOsService _osService;
        private readonly ITaskService _taskService;
        private readonly ITopologyService _topologyService;
        private readonly IStatisticsService _statisticsService;

        private static IReadOnlyDictionary<string, LambdaExpression> CpuExpressions { get; } =
            new Dictionary<string, LambdaExpression>
            {
                [FilterHelpers.TypeMap[FilterHelpers.Type.CpuBrand]] =
                    GetResultExpression(model => model.Topology.Cpu.Brand),
                [FilterHelpers.TypeMap[FilterHelpers.Type.CpuVendor]] =
                    GetResultExpression(model => model.Topology.Cpu.Vendor),
                [FilterHelpers.TypeMap[FilterHelpers.Type.CpuFrequency]] =
                    GetResultExpression(model => model.Topology.Cpu.Frequency)
            };

        private static IReadOnlyDictionary<string, LambdaExpression> TopologyExpressions { get; } =
            new Dictionary<string, LambdaExpression>
            {
                [FilterHelpers.TypeMap[FilterHelpers.Type.CpuCores]] =
                    GetResultExpression(model => model.Topology.TotalPhysicalCores),
                [FilterHelpers.TypeMap[FilterHelpers.Type.CpuLogicalCores]] =
                    GetResultExpression(model => model.Topology.TotalLogicalCores)
            };

        private static IReadOnlyDictionary<string, LambdaExpression> ResultExpressions { get; } =
            new Dictionary<string, LambdaExpression>
            {
                [FilterHelpers.TypeMap[FilterHelpers.Type.MemorySize]] = GetResultExpression(model => model.MemorySize),
                [FilterHelpers.TypeMap[FilterHelpers.Type.Timestamp]] = GetResultExpression(model => model.TimeStamp),
                ["startTime".ToLowerInvariant()] = GetResultExpression(model => model.TimeStamp),
                ["endTime".ToLowerInvariant()] = GetResultExpression(model => model.TimeStamp),
                [FilterHelpers.TypeMap[FilterHelpers.Type.Name].ToLowerInvariant()] =
                    GetResultExpression(model => model.Benchmark.Name),
                [FilterHelpers.TypeMap[FilterHelpers.Type.OsCategory]] =
                    GetResultExpression(model => model.Os.Category),
                [FilterHelpers.TypeMap[FilterHelpers.Type.OsName]] = GetResultExpression(model => model.Os.Name),
                [FilterHelpers.TypeMap[FilterHelpers.Type.OsVersion]] = GetResultExpression(model => model.Os.Version)
            };

        private static IReadOnlyDictionary<string, LambdaExpression> ResultProjectionExpressions { get; } =
            ResultExpressions.Concat(CpuExpressions)
                .Concat(TopologyExpressions)
                .ToDictionary(x => x.Key, x => x.Value);

        private static LambdaExpression GetResultExpression<T>(Expression<Func<BenchmarkResultModel, T>> baseExp)
        {
            // Dirty hack to prevent boxing of values
            return baseExp;
        }

        #region IResultsService Members

        public BenchmarkResultService(IBenchmarkService benchmarkService, ITopologyService topologyService,
            IElpidaService elpidaService, ITaskService taskService, IOsService osService,
            IBenchmarkResultsRepository benchmarkResultsRepository, ICpuService cpuService, IStatisticsService statisticsService)
        {
            _benchmarkService = benchmarkService;
            _topologyService = topologyService;
            _elpidaService = elpidaService;
            _taskService = taskService;
            _osService = osService;
            _benchmarkResultsRepository = benchmarkResultsRepository;
            _cpuService = cpuService;
            _statisticsService = statisticsService;
        }

        public async Task<long> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
        {
            var benchmark = await _benchmarkService.GetSingleAsync(resultDto.Result.Uuid, cancellationToken);
            var cpuId = await _cpuService.GetOrAddCpuAsync(resultDto.System.Cpu, cancellationToken);
            var topologyId = await _topologyService.GetOrAddTopologyAsync(cpuId, resultDto.System.Topology, cancellationToken);
            var elpidaId = await _elpidaService.GetOrAddElpidaAsync(resultDto.Elpida, cancellationToken);
            var osId = await _osService.GetOrAddOsAsync(resultDto.System.Os, cancellationToken);
            
            var taskResults = new List<TaskResultModel>();
            foreach (var taskResult in resultDto.Result.TaskResults)
            {
                var task = await _taskService.GetSingleAsync(taskResult.Uuid, cancellationToken);
                taskResults.Add(taskResult.ToModel(task.Id, topologyId, cpuId));
            }

            var resultModel = resultDto.ToModel(benchmark.Id, topologyId, osId, elpidaId, taskResults);

            var result = await _benchmarkResultsRepository.CreateAsync(resultModel, cancellationToken);

            resultDto.System.Cpu.Id = cpuId;
            await _statisticsService.AddBenchmarkResultAsync(resultDto, cancellationToken);
            
            await _benchmarkResultsRepository.SaveChangesAsync(cancellationToken);

            return result.Id;
        }

        public async Task<ResultDto> GetSingleAsync(long id, CancellationToken cancellationToken)
        {
            var resultModel = await _benchmarkResultsRepository.GetSingleAsync(id, cancellationToken);

            if (resultModel == null) throw new NotFoundException(id.ToString());

            return resultModel.ToDto();
        }

        public async Task<PagedResult<ResultPreviewDto>> GetPagedAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken)
        {
            var expressionBuilder = new QueryExpressionBuilder(ResultProjectionExpressions);

            var result = await _benchmarkResultsRepository.GetPagedPreviewsAsync(
                queryRequest.PageRequest.Next,
                queryRequest.PageRequest.Count,
                queryRequest.Descending,
                expressionBuilder.GetOrderBy<BenchmarkResultModel>(queryRequest),
                expressionBuilder.Build<BenchmarkResultModel>(queryRequest.Filters),
                queryRequest.PageRequest.TotalCount == 0,
                cancellationToken);

            queryRequest.PageRequest.TotalCount = result.TotalCount;

            return new PagedResult<ResultPreviewDto>(result.Items.Select(m => m.ToDto()).ToList(),
                queryRequest.PageRequest);
        }

        #endregion
    }
}