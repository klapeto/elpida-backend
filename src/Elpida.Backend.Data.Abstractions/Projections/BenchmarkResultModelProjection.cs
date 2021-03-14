using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data.Abstractions.Projections
{
	public class BenchmarkResultModelProjection
	{
		public BenchmarkModel Benchmark { get; set; } = new BenchmarkModel();

		public IList<TaskResultModelProjection> TaskResults { get; set; } = new List<TaskResultModelProjection>();
	}
}