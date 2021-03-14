using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Extensions.Benchmark
{
	public static class BenchmarkDtoExtensions
	{
		public static BenchmarkDto ToDto(this BenchmarkModel model)
		{
			return new BenchmarkDto
			{
				Id = model.Id,
				Name = model.Name,
				TaskSpecifications = model.TaskSpecifications
			};
		}

		public static BenchmarkModel ToModel(this BenchmarkDto dto)
		{
			return new BenchmarkModel
			{
				Id = dto.Id,
				Name = dto.Name,
				TaskSpecifications = dto.TaskSpecifications
			};
		}
	}
}