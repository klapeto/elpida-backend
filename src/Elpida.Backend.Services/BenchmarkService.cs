using System;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Benchmark;

namespace Elpida.Backend.Services
{
    public class BenchmarkService: IBenchmarkService
    {
        private readonly IBenchmarkRepository _benchmarkRepository;

        public BenchmarkService(IBenchmarkRepository benchmarkRepository)
        {
            _benchmarkRepository = benchmarkRepository;
        }

        public async Task<BenchmarkDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
        {
            var benchmarkModel = await _benchmarkRepository.GetSingleAsync(t => t.Uuid == uuid, cancellationToken);

            if (benchmarkModel == null) throw new NotFoundException("Task was not found.", uuid);

            return benchmarkModel.ToDto();
        }
    }
}