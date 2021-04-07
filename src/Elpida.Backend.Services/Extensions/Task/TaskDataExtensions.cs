using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Task
{
    public static class TaskDataExtensions
    {
        private static DataSpecificationDto? CreateInputSpecDto(this TaskModel model)
        {
            if (string.IsNullOrWhiteSpace(model.InputName)) return null;

            return new DataSpecificationDto
            {
                Name = model.InputName,
                Description = model.InputDescription!,
                Unit = model.InputDescription!,
                RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.InputProperties!)
            };
        }

        private static DataSpecificationDto? CreateOutputSpecDto(this TaskModel model)
        {
            if (string.IsNullOrWhiteSpace(model.OutputName)) return null;

            return new DataSpecificationDto
            {
                Name = model.OutputName,
                Description = model.OutputDescription!,
                Unit = model.OutputUnit!,
                RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.OutputProperties!)
            };
        }

        public static TaskDto ToDto(this TaskModel taskModel)
        {
            return new TaskDto
            {
                Id = taskModel.Id,
                Uuid = taskModel.Uuid,
                Name = taskModel.Name,
                Description = taskModel.Description,
                Input = CreateInputSpecDto(taskModel),
                Output = CreateOutputSpecDto(taskModel),
                Result = new ResultSpecificationDto
                {
                    Name = taskModel.ResultName,
                    Description = taskModel.ResultDescription,
                    Aggregation = taskModel.ResultAggregation,
                    Type = taskModel.ResultType,
                    Unit = taskModel.ResultUnit
                }
            };
        }
    }
}