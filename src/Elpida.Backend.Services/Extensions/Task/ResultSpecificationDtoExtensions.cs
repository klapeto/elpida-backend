using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Extensions.Task
{
	public static class ResultSpecificationDtoExtensions
	{
		public static ResultSpecificationDto ToDto(this ResultSpecificationModel model)
		{
			return new ResultSpecificationDto
			{
				Name = model.Name,
				Description = model.Description,
				Aggregation = model.Aggregation,
				Type = model.Type,
				Unit = model.Unit
			};
		}

		public static ResultSpecificationModel ToModel(this ResultSpecificationDto dto)
		{
			return new ResultSpecificationModel
			{
				Name = dto.Name,
				Description = dto.Description,
				Unit = dto.Unit,
				Aggregation = dto.Aggregation,
				Type = dto.Type,
			};
		}
	}
}