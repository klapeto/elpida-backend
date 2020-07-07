namespace Elpida.Backend.Data.Abstractions.Models
{
	public class TopologyModel
	{
		public uint TotalLogicalCores { get; set; }
		public uint TotalPhysicalCores { get; set; }
		public uint TotalDepth { get; set; }
		public CpuNodeModel Root { get; set; }
	}
}