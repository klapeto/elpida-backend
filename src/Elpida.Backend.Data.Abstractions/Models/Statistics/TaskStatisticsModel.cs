using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Task;

namespace Elpida.Backend.Data.Abstractions.Models.Statistics
{
    public class TaskStatisticsModel : Entity
    {
        public long TaskId { get; set; }
        public TaskModel Task { get; set; } = default!;
        
        public long CpuId { get; set; }
        public CpuModel Cpu { get; set; } = default!;

        public double TotalValue { get; set; }
        public double TotalDeviation { get; set; }
        
        public long SampleSize { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public double Mean { get; set; }
        public double StandardDeviation { get; set; }
        public double Tau { get; set; }
        public double MarginOfError { get; set; }
    }
}