using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services.Extensions.Benchmark
{
	public static class BenchmarkDtoExtensions
	{
		public static BenchmarkDto ToDto(this BenchmarkModel model)
		{
			return new BenchmarkDto
			{
				Id = model.Id,
				Uuid = model.Uuid,
				Name = model.Name,
				TaskSpecifications = model.Tasks.Select(t => t.ToDto()).ToList()
			};
		}

		public static void Update(this BenchmarkModel model, BenchmarkModel other)
		{
			model.Name = other.Name;
			model.Uuid = other.Uuid;
			model.Tasks = other.Tasks.ToList();
		}

		public static BenchmarkModel ToModel(this BenchmarkDto dto)
		{
			return new BenchmarkModel
			{
				Id = dto.Id,
				Uuid = dto.Uuid,
				Name = dto.Name,
				Tasks = dto.TaskSpecifications.Select(t => t.ToModel()).ToList()
			};
		}
	}
}