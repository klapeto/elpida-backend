using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Benchmark;

namespace Elpida.Backend.Services
{
    public class BenchmarkService : Service<BenchmarkDto, BenchmarkModel, IBenchmarkRepository>, IBenchmarkService
    {
        public BenchmarkService(IBenchmarkRepository benchmarkRepository)
            : base(benchmarkRepository)
        {
        }

        private static IEnumerable<FilterExpression> BenchmarkExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("benchmarkName", model => model.Name)
        };

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return BenchmarkExpressions;
        }

        protected override BenchmarkDto ToDto(BenchmarkModel model)
        {
            return model.ToDto();
        }

        protected override BenchmarkModel ToModel(BenchmarkDto dto)
        {
            return dto.ToDto();
        }

        protected override Expression<Func<BenchmarkModel, bool>> GetCreationBypassCheckExpression(BenchmarkDto dto)
        {
            return model => model.Uuid == dto.Uuid;
        }

        public async Task<BenchmarkDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
        {
            var model = await Repository.GetSingleAsync(b => b.Uuid == uuid, cancellationToken);

            if (model == null) throw new NotFoundException("Benchmark was not found.", uuid);

            return model.ToDto();
        }
    }
}