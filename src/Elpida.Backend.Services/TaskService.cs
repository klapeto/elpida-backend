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