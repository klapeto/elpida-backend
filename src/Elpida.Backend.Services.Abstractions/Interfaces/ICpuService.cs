using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface ICpuService : IService<CpuDto>
    {
        Task<IEnumerable<TaskStatisticsDto>> GetStatisticsAsync(long cpuId, CancellationToken cancellationToken = default);
        Task UpdateBenchmarkStatisticsAsync(long cpuId, BenchmarkResultDto benchmarkResult, CancellationToken cancellationToken = default);
    }
}