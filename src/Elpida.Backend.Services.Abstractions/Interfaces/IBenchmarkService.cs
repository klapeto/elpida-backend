using System;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface IBenchmarkService : IService<BenchmarkDto>
    {
        Task<BenchmarkDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default);
    }
}