using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class SystemModelProjection
	{
		public CpuModel Cpu { get; set; }
		public TopologyModel Topology { get; set; }
		public OsModel Os { get; set; }
		public MemoryModel Memory { get; set; }
	}
}