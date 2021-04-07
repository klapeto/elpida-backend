using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface IStatisticsService
    {
        Task AddBenchmarkResultAsync(ResultDto result, CancellationToken cancellationToken = default);
    }
}