using System;
using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class ResultModel
	{
		public string Id { get; set; }

		public DateTime TimeStamp { get; set; }
		
		public IList<ulong> Affinity { get; set; }
		public ElpidaModel Elpida { get; set; }
		public SystemModel System { get; set; }
		public BenchmarkResultModel Result { get; set; }
	}
}