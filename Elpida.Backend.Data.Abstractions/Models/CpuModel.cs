using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class CpuModel
	{
		public string Vendor { get; set; }
		public string Brand { get; set; }
		public int Model { get; set; }
		public int Family { get; set; }
		public int Stepping { get; set; }
		public ulong Frequency { get; set; }
		public bool TurboBoost { get; set; }
		public bool TurboBoost3 { get; set; }
		public bool Smt { get; set; }
		public IList<CpuCacheModel> Caches { get; set; }
		public IList<string> Features { get; set; }
	}
}