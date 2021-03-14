using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions.Models.Task
{
	public class DataSpecificationModel
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Unit { get; set; } = string.Empty;
		public IList<string> RequiredProperties { get; set; } = new List<string>();
	}
}