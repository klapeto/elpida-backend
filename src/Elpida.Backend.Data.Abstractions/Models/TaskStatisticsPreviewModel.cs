namespace Elpida.Backend.Data.Abstractions.Models
{
    public class TaskStatisticsPreviewModel
    {
        public string CpuVendor { get; set; } = default!;
        public string CpuBrand { get; set; } = default!;
        public string TaskName { get; set; } = default!;
        public string TaskResultUnit { get; set; } = default!;
        public int CpuCores { get; set; }
        public int CpuLogicalCores { get; set; }
        public string TopologyHash { get; set; } = default!;
        public double Mean { get; set; }
        public long SampleSize { get; set; }
    }
}