using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class BenchmarkDto
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public IList<string> TaskSpecifications { get; set; } = new List<string>();
	}
}