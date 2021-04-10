using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services.Extensions.Benchmark
{
    public static class BenchmarkDataExtensions
    {
        public static BenchmarkDto ToDto(this BenchmarkModel benchmarkModel)
        {
            return new BenchmarkDto
            {
                Id = benchmarkModel.Id,
                Uuid = benchmarkModel.Uuid,
                Name = benchmarkModel.Name,
                TaskSpecifications = benchmarkModel.Tasks
                    .Select(t => t.ToDto())
                    .ToList()
            };
        }
        
        public static BenchmarkModel ToDto(this BenchmarkDto benchmarkDto)
        {
            return new BenchmarkModel
            {
                Id = benchmarkDto.Id,
                Uuid = benchmarkDto.Uuid,
                Name = benchmarkDto.Name,
                Tasks = benchmarkDto.TaskSpecifications.Select(t => t.ToModel()).ToList()
            };
        }
    }
}