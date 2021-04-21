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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public class CpuService : Service<CpuDto, CpuModel, ICpuRepository>, ICpuService
    {
        private readonly ICpuRepository _cpuRepository;
        private readonly ITaskService _taskService;

        public CpuService(ICpuRepository cpuRepository, ITaskService taskService)
            : base(cpuRepository)
        {
            _cpuRepository = cpuRepository;
            _taskService = taskService;
        }

        private static IEnumerable<FilterExpression> CpuExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("cpuBrand", model => model.Brand),
            CreateFilter("cpuVendor", model => model.Vendor),
            CreateFilter("cpuFrequency", model => model.Frequency)
        };

        public async Task<IEnumerable<TaskStatisticsDto>> GetStatisticsAsync(long cpuId,
            CancellationToken cancellationToken = default)
        {
            var cpuModel = await _cpuRepository.GetSingleAsync(cpuId, cancellationToken);

            if (cpuModel == null) throw new NotFoundException("Cpu was not found.", cpuId);

            return cpuModel.TaskStatistics.Select(s => s.ToDto());
        }

        public async Task UpdateBenchmarkStatisticsAsync(long cpuId, BenchmarkResultDto benchmarkResult,
            CancellationToken cancellationToken = default)
        {
            var cpuModel = await _cpuRepository.GetSingleAsync(cpuId, cancellationToken);

            if (cpuModel == null) throw new NotFoundException("Cpu was not found.", cpuId);

            foreach (var taskResult in benchmarkResult.TaskResults)
            {
                var task = await _taskService.GetSingleAsync(taskResult.Uuid, cancellationToken);
                var stats = cpuModel.TaskStatistics.FirstOrDefault(t => t.TaskId == task.Id);
                if (stats == null)
                {
                    stats = new TaskStatisticsModel
                    {
                        Cpu = cpuModel,
                        Max = taskResult.Value,
                        Mean = taskResult.Value,
                        Min = taskResult.Value,
                        TaskId = task.Id,
                        SampleSize = 1,
                        TotalDeviation = 0,
                        TotalValue = taskResult.Value,
                        Tau = StatisticsHelpers.CalculateTau(1),
                        StandardDeviation = 0,
                        MarginOfError = 0
                    };
                    cpuModel.TaskStatistics.Add(stats);
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

            await _cpuRepository.SaveChangesAsync(cancellationToken);
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return CpuExpressions;
        }

        protected override CpuDto ToDto(CpuModel model)
        {
            return model.ToDto();
        }

        protected override CpuModel ToModel(CpuDto dto)
        {
            return dto.ToModel();
        }

        protected override Expression<Func<CpuModel, bool>> GetCreationBypassCheckExpression(CpuDto dto)
        {
            var additionalInfo = dto.ToModel().AdditionalInfo;
            return model =>
                model.Vendor == dto.Vendor
                && model.Brand == dto.Brand
                && model.AdditionalInfo == additionalInfo;
        }
    }
}