// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Task;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Task;
using Elpida.Backend.Services.Utilities;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
	public class TaskService : Service<TaskDto, TaskModel, ITaskRepository>, ITaskService
	{
		public TaskService(ITaskRepository taskRepository)
			: base(taskRepository)
		{
		}

		private static IEnumerable<FilterExpression> ResultFilters { get; } = new List<FilterExpression>
		{
			FiltersTransformer.CreateFilter<TaskModel, string>("taskName", model => model.Name),
		};

		public async Task<TaskDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
		{
			var task = await Repository.GetSingleAsync(m => m.Uuid == uuid, cancellationToken);

			if (task == null)
			{
				throw new NotFoundException("Task was not found.", uuid);
			}

			return ToDto(task);
		}

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			return ResultFilters;
		}

		protected override TaskDto ToDto(TaskModel model)
		{
			return model.ToDto();
		}

		protected override Task<TaskModel> ProcessDtoAndCreateModelAsync(
			TaskDto dto,
			CancellationToken cancellationToken
		)
		{
			return Task.FromResult(
				new TaskModel
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
					ResultDescription = dto.Result.Description,
					ResultType = (int)dto.Result.Type,
					ResultAggregation = (int)dto.Result.Aggregation,
					ResultUnit = dto.Result.Unit,
				}
			);
		}

		protected override Expression<Func<TaskModel, bool>> GetCreationBypassCheckExpression(TaskDto dto)
		{
			return model => model.Uuid == dto.Uuid;
		}
	}
}