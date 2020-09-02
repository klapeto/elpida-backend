namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class SystemDto
	{
		public CpuDto Cpu { get; set; }
		public OsDto Os { get; set; }
		public TopologyDto Topology { get; set; }
		public MemoryDto Memory { get; set; }
	}
}