using System;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
    public interface ITaskService : IService<TaskDto>
    {
        Task<TaskDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default);
    }
}