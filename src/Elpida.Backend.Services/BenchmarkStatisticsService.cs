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
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Benchmark;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Extensions.Topology;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public class BenchmarkStatisticsService :
        Service<BenchmarkStatisticsDto, BenchmarkStatisticsModel, IBenchmarkStatisticsRepository>,
        IBenchmarkStatisticsService
    {
        private readonly IBenchmarkService _benchmarkService;
        private readonly ICpuService _cpuService;
        private readonly ITopologyService _topologyService;

        public BenchmarkStatisticsService(IBenchmarkService benchmarkService,
            ITopologyService topologyService,
            IBenchmarkStatisticsRepository benchmarkStatisticsRepository,
            ICpuService cpuService)
            : base(benchmarkStatisticsRepository)
        {
            _topologyService = topologyService;
            _cpuService = cpuService;
            _benchmarkService = benchmarkService;
        }

        private static IEnumerable<FilterExpression> StatisticsExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("cpuId", model => model.CpuId),
            CreateFilter("topologyId", model => model.TopologyId),
            CreateFilter("benchmarkId", model => model.BenchmarkId)
        };

        public async Task UpdateTaskStatisticsAsync(ResultDto resultDto,
            CancellationToken cancellationToken = default)
        {
            var topology = await _topologyService.GetSingleAsync(resultDto.System.Topology.Id, cancellationToken);
            var benchmark = await _benchmarkService.GetSingleAsync(resultDto.Result.Uuid, cancellationToken);

            var stats = await Repository.GetSingleAsync(
                t => t.BenchmarkId == benchmark.Id
                     && t.TopologyId == topology.Id, cancellationToken);
            if (stats == null)
            {
                stats = new BenchmarkStatisticsModel
                {
                    CpuId = topology.CpuId,
                    Max = resultDto.Result.Score,
                    Mean = resultDto.Result.Score,
                    Min = resultDto.Result.Score,
                    BenchmarkId = benchmark.Id,
                    TopologyId = topology.Id,
                    SampleSize = 1,
                    TotalDeviation = 0,
                    TotalValue = resultDto.Result.Score,
                    Tau = StatisticsHelpers.CalculateTau(1),
                    StandardDeviation = 0,
                    MarginOfError = 0,
                };
                await Repository.CreateAsync(stats, cancellationToken);
            }
            else
            {
                stats.Max = Math.Max(stats.Max, resultDto.Result.Score);
                stats.Min = Math.Min(stats.Min, resultDto.Result.Score);
                stats.TotalValue += resultDto.Result.Score;
                stats.SampleSize++;
                stats.Mean = stats.TotalValue / stats.SampleSize;
                stats.TotalDeviation += Math.Pow(resultDto.Result.Score - stats.Mean, 2.0);
                stats.StandardDeviation = Math.Sqrt(stats.TotalDeviation / stats.SampleSize);
                stats.MarginOfError = stats.StandardDeviation / Math.Sqrt(stats.SampleSize);
                stats.Tau = StatisticsHelpers.CalculateTau(stats.SampleSize);

            }

            // TODO: concurrency
            await Repository.SaveChangesAsync(cancellationToken);
        }

        public Task<PagedResult<BenchmarkStatisticsPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionsAsync(queryRequest, m => new BenchmarkStatisticsPreviewDto
            {
                Id = m.Id,
                CpuVendor = m.Cpu.Vendor,
                CpuBrand = m.Cpu.Brand,
                CpuCores = m.Topology.TotalPhysicalCores,
                CpuLogicalCores = m.Topology.TotalLogicalCores,
                BenchmarkName = m.Benchmark.Name,
                BenchmarkUuid = m.Benchmark.Uuid,
                SampleSize = m.SampleSize,
                TopologyHash = m.Topology.TopologyHash,
                BenchmarkScoreUnit = m.Benchmark.ScoreUnit,
                Mean = m.Mean,
                Comparison = m.Benchmark.ScoreComparison,
            }, cancellationToken);
        }

        protected override Task<BenchmarkStatisticsModel> ProcessDtoAndCreateModelAsync(BenchmarkStatisticsDto dto,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new BenchmarkStatisticsModel
            {
                Id = dto.Id,
                CpuId = dto.Cpu.Id,
                TopologyId = dto.Topology.Id,
                BenchmarkId = dto.Benchmark.Id,
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
            return StatisticsExpressions
                .Concat(_cpuService.GetFilters<BenchmarkStatisticsModel, CpuModel>(m => m.Cpu))
                .Concat(_topologyService.GetFilters<BenchmarkStatisticsModel, TopologyModel>(m => m.Topology))
                .Concat(_benchmarkService.GetFilters<BenchmarkStatisticsModel, BenchmarkModel>(m => m.Benchmark));
        }

        protected override BenchmarkStatisticsDto ToDto(BenchmarkStatisticsModel model)
        {
            return new BenchmarkStatisticsDto
            {
                Id = model.Id,
                Cpu = model.Cpu.ToDto(),
                Benchmark = model.Benchmark.ToDto(),
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