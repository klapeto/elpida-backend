namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class TopologyDto
	{
		public uint TotalLogicalCores { get; set; }
		public uint TotalPhysicalCores { get; set; }
		public uint TotalDepth { get; set; }
		public CpuNodeDto Root { get; set; }
	}
}