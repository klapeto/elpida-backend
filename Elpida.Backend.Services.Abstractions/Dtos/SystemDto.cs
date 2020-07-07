namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class SystemDto
	{
		public CpuDto Cpu { get; set; }
		public OsDto Os { get; set; }
		public TopologyDto Topology { get; set; }
		public MemoryDto Memory { get; set; }
	}
}