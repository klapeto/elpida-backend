namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class CpuCacheDto
	{
		public string Name { get; set; }
		public string Associativity { get; set; }
		public ulong Size { get; set; }
		public uint LinesPerTag { get; set; }
		public uint LineSize { get; set; }
	}
}