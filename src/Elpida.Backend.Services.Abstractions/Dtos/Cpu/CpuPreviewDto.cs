namespace Elpida.Backend.Services.Abstractions.Dtos.Cpu
{
    public class CpuPreviewDto : FountationDto
    {
        public string Vendor { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public int TopologiesCount { get; set; }
        public int TaskStatisticsCount { get; set; }
    }
}