namespace Elpida.Backend.Services.Abstractions.Dtos
{
    public class CpuStatisticsPreviewDto
    {
        public string CpuVendor { get; set; } = default!;
        public string CpuBrand { get; set; } = default!;
        public int Tasks { get; set; }
        public int Topologies { get; set; }
    }
}