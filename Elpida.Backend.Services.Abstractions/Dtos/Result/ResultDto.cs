using System;
using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class ResultDto
	{
		public string Id { get; set; }
		public DateTime TimeStamp { get; set; }
		public IList<ulong> Affinity { get; set; }
		public ElpidaDto Elpida { get; set; }
		public SystemDto System { get; set; }
		public BenchmarkResultDto Result { get; set; }
	}
}