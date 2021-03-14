using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class TaskResultDataExtensions
	{
		public static TaskResultDto ToDto(this TaskResultModel model, TaskModel taskModel)
		{
			return new TaskResultDto
			{
				Id = taskModel.Id,
				Name = taskModel.Name,
				Description = taskModel.Description,
				Input = taskModel.Input?.ToDto(),
				Output = taskModel.Output?.ToDto(),
				Result = taskModel.Result.ToDto(),
				Outliers = model.Outliers.Select(o => o.ToDto()).ToList(),
				Statistics = model.Statistics.ToDto(),
				Time = model.Time,
				Value = model.Value,
				InputSize = model.InputSize
			};
		}
		
		public static TaskResultModel ToModel(this TaskResultDto taskResultDto)
		{
			return new TaskResultModel
			{
				TaskId = taskResultDto.Id,
				Time = taskResultDto.Time,
				Value = taskResultDto.Value,
				InputSize = taskResultDto.InputSize,
				Outliers = taskResultDto.Outliers.Select(dto => dto.ToModel()).ToList(),
				Statistics = taskResultDto.Statistics.ToModel()
			};
		}
	}
}