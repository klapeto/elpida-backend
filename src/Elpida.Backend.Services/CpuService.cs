using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public class CpuService : ICpuService
    {
        private readonly ICpuRepository _cpuRepository;
        private readonly ITaskService _taskService;

        public CpuService(ICpuRepository cpuRepository, ITaskService taskService)
        {
            _cpuRepository = cpuRepository;
            _taskService = taskService;
        }

        public async Task<long> GetOrAddCpuAsync(CpuDto cpu, CancellationToken cancellationToken)
        {
            var additionalInfo = cpu.ToModel().AdditionalInfo;

            var cpuModel = await _cpuRepository.GetSingleAsync(model =>
                model.Vendor == cpu.Vendor
                && model.Brand == cpu.Brand
                && model.AdditionalInfo == additionalInfo, cancellationToken);

            if (cpuModel != null) return cpuModel.Id;

            cpu.Id = 0;
            cpuModel = cpu.ToModel();
            cpuModel = await _cpuRepository.CreateAsync(cpuModel, cancellationToken);
            
            await _cpuRepository.SaveChangesAsync(cancellationToken);

            return cpuModel.Id;
        }

        public async Task<CpuDto> GetSingleAsync(long cpuId, CancellationToken cancellationToken = default)
        {
            var cpuModel = await _cpuRepository.GetSingleAsync(cpuId, cancellationToken);

            if (cpuModel == null) throw new NotFoundException("Cpu was not found.", cpuId);

            return cpuModel.ToDto();
        }

        public async Task<IEnumerable<TaskStatisticsDto>> GetStatisticsAsync(long cpuId, CancellationToken cancellationToken = default)
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
                        MarginOfError = 0,
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
    }
}