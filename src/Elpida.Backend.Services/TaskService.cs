using System;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services
{
    public class TaskService: ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
        {
            var taskModel = await _taskRepository.GetSingleAsync(t => t.Uuid == uuid, cancellationToken);

            if (taskModel == null) throw new NotFoundException("Task was not found.", uuid);

            return taskModel.ToDto();
        }
    }
}