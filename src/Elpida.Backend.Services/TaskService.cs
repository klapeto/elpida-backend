/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services
{
    public class TaskService : Service<TaskDto, TaskModel, ITaskRepository>, ITaskService
    {
        public TaskService(ITaskRepository taskRepository)
            : base(taskRepository)
        {
        }

        public async Task<TaskDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
        {
            var model = await Repository.GetSingleAsync(b => b.Uuid == uuid, cancellationToken);

            if (model == null) throw new NotFoundException("Task was not found.", uuid);

            return model.ToDto();
        }

        protected override TaskDto ToDto(TaskModel model)
        {
            return model.ToDto();
        }

        protected override TaskModel ToModel(TaskDto dto)
        {
            return dto.ToModel();
        }

        protected override Expression<Func<TaskModel, bool>> GetCreationBypassCheckExpression(TaskDto dto)
        {
            return model => model.Uuid == dto.Uuid;
        }
    }
}