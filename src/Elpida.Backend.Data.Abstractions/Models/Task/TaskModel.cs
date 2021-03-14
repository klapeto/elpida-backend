using Elpida.Backend.Data.Abstractions.Interfaces;

namespace Elpida.Backend.Data.Abstractions.Models.Task
{
	public class TaskModel : IEntity
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public ResultSpecificationModel Result { get; set; } = new ResultSpecificationModel();
		public DataSpecificationModel? Input { get; set; }
		public DataSpecificationModel? Output { get; set; }
	}
}