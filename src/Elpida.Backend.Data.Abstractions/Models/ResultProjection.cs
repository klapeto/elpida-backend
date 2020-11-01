using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class ResultProjection
	{
		public string Id { get; set; }

		public DateTime TimeStamp { get; set; }
		
		public IList<ulong> Affinity { get; set; }
		public ElpidaModel Elpida { get; set; }
		public SystemModelProjection System { get; set; }
		public BenchmarkResultModel Result { get; set; }
	}
	
}