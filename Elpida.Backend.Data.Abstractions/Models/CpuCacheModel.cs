namespace Elpida.Backend.Data.Abstractions.Models
{
	public class CpuCacheModel
	{
		public string Name { get; set; }
		public string Associativity { get; set; }
		public ulong Size { get; set; }
		public uint LinesPerTag { get; set; }
		public uint LineSize { get; set; }
	}
}