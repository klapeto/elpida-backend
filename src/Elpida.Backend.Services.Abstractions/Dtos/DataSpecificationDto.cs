using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class DataSpecificationDto
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Unit { get; set; } = string.Empty;
		public IList<string> RequiredProperties { get; set; } = new List<string>();
	}
}