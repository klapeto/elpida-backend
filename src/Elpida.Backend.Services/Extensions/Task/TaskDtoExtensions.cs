using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Extensions.Task
{
	public static class TaskDtoExtensions
	{
		public static TaskDto ToDto(this TaskModel model)
		{
			return new TaskDto
			{
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				Input = model.Input?.ToDto(),
				Output = model.Output?.ToDto(),
				Result = model.Result.ToDto()
			};
		}
		
		public static TaskModel ToModel(this TaskDto dto)
		{
			return new TaskModel
			{
				Id = dto.Id,
				Name = dto.Name,
				Description = dto.Description,
				Input = dto.Input?.ToModel(),
				Output = dto.Output?.ToModel(),
				Result = dto.Result.ToModel(),
			};
		}
	}
}