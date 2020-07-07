using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class BenchmarkResultModel
	{
		public string Name { get; set; }
		public IList<TaskResultModel> TaskResults { get; set; }
	}
}