using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class BenchmarkResultDto
	{
		public string Name { get; set; }
		public IList<TaskResultDto> TaskResults { get; set; }
	}
}