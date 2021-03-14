using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions.Models.Statistics
{
	public class CpuStatisticsModel : IEntity
	{
		public string Id { get; set; } = string.Empty;
		
		public string CpuId { get; set; } = string.Empty;
		
		public string BenchmarkId { get; set; } = string.Empty;
	}
}