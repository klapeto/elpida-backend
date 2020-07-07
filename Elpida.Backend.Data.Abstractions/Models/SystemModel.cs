namespace Elpida.Backend.Data.Abstractions.Models
{
	public class SystemModel
	{
		public CpuModel Cpu { get; set; }
		public OsModel Os { get; set; }
		public TopologyModel Topology { get; set; }
		public MemoryModel Memory { get; set; }
	}
}