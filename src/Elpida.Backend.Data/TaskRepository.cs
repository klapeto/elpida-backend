using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;

namespace Elpida.Backend.Data
{
	public class TaskRepository : EntityRepository<TaskModel>, ITaskRepository
	{
		public TaskRepository(ElpidaContext context)
			: base(context, context.Tasks)
		{
		}
	}
}