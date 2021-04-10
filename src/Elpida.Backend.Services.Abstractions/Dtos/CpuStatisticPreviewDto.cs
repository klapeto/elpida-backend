namespace Elpida.Backend.Services.Abstractions.Dtos
{
    public class CpuStatisticPreviewDto
    {
        public long CpuId { get; set; }
        public string Vendor { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public long Tasks { get; set; }
    }
}