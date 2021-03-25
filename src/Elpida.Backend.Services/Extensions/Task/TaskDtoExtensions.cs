/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos;
using Newtonsoft.Json;

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
				Input = model.InputName != null
					? new DataSpecificationDto
					{
						Name = model.InputName,
						Description = model.InputDescription!,
						Unit = model.InputDescription!,
						RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.InputProperties!)
					}
					: null,
				Output = model.OutputName != null
					? new DataSpecificationDto
					{
						Name = model.OutputName,
						Description = model.OutputDescription!,
						Unit = model.OutputDescription!,
						RequiredProperties = JsonConvert.DeserializeObject<List<string>>(model.OutputProperties!)
					}
					: null,
				Result = new ResultSpecificationDto
				{
					Name = model.ResultName,
					Description = model.ResultDescription,
					Aggregation = model.ResultAggregation,
					Type = model.ResultType,
					Unit = model.ResultUnit
				}
			};
		}

		public static void Update(this TaskModel model, TaskModel other)
		{
			model.Uuid = other.Uuid;
			model.Name = other.Name;
			model.Description = other.Description;

			model.InputName = other.InputName;
			model.InputDescription = other.InputDescription;
			model.InputUnit = other.InputUnit;
			model.InputProperties = JsonConvert.SerializeObject(other.InputProperties);

			model.OutputName = other.OutputName;
			model.OutputDescription = other.OutputDescription;
			model.OutputUnit = other.OutputUnit;
			model.OutputProperties = JsonConvert.SerializeObject(other.OutputProperties);

			model.ResultName = other.ResultName;
			model.ResultDescription = other.Description;
			model.ResultAggregation = other.ResultAggregation;
			model.ResultType = other.ResultType;
			model.ResultUnit = other.ResultUnit;
		}

		public static TaskModel ToModel(this TaskDto dto)
		{
			return new TaskModel
			{
				Id = dto.Id,
				Uuid = dto.Uuid,
				Name = dto.Name,
				Description = dto.Description,

				InputName = dto.Input?.Name,
				InputDescription = dto.Input?.Description,
				InputUnit = dto.Input?.Unit,
				InputProperties = JsonConvert.SerializeObject(dto.Input?.RequiredProperties),

				OutputName = dto.Output?.Name,
				OutputDescription = dto.Output?.Description,
				OutputUnit = dto.Output?.Unit,
				OutputProperties = JsonConvert.SerializeObject(dto.Output?.RequiredProperties),

				ResultName = dto.Result.Name,
				ResultDescription = dto.Description,
				ResultAggregation = dto.Result.Aggregation,
				ResultType = dto.Result.Type,
				ResultUnit = dto.Result.Unit
			};
		}
	}
}