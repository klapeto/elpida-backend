using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;

namespace Elpida.Backend.Data.Abstractions.Projections
{
	public class TaskResultModelProjection
	{
		public TaskModel Task { get; set; } = new TaskModel();
		public TaskResultModel Result { get; set; } = new TaskResultModel();
	}
}