using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions.Projections
{
	public class ResultProjection
	{
		public string Id { get; set; } = string.Empty;

		public DateTime TimeStamp { get; set; }

		public IList<long> Affinity { get; set; } = new List<long>();
		public ElpidaModel Elpida { get; set; } = new ElpidaModel();
		public SystemModelProjection System { get; set; } = new SystemModelProjection();
		public BenchmarkResultModelProjection Result { get; set; } = new BenchmarkResultModelProjection();
	}
}