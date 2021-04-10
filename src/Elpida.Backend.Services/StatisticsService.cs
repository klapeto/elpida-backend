using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;

namespace Elpida.Backend.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ICpuService _cpuService;

        public StatisticsService(ICpuService cpuService)
        {
            _cpuService = cpuService;
        }

        public Task AddBenchmarkResultAsync(ResultDto result, CancellationToken cancellationToken = default)
        {
            return _cpuService.UpdateBenchmarkStatisticsAsync(result.System.Cpu.Id, result.Result, cancellationToken);
        }

        public async Task<CpuStatisticPreviewDto> GetPreviewsByCpuAsync(QueryRequest queryRequest, CancellationToken cancellationToken = default)
        {
            return new CpuStatisticPreviewDto();
            // var x = _cpuService
        }
    }
}