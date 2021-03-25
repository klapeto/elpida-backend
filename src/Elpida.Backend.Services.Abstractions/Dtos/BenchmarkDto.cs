using System;
using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class BenchmarkDto
	{
		public long Id { get; set; }
		public Guid Uuid { get; set; }
		public string Name { get; set; } = string.Empty;
		public IList<TaskDto> TaskSpecifications { get; set; } = new List<TaskDto>();
	}
}