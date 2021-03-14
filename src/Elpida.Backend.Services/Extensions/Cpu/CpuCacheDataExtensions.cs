using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Cpu
{
	public static class CpuCacheDataExtensions
	{
		public static CpuCacheDto ToDto(this CpuCacheModel model)
		{
			return new CpuCacheDto
			{
				Associativity = model.Associativity,
				Name = model.Name,
				Size = model.Size,
				LineSize = model.LineSize,
				LinesPerTag = model.LinesPerTag
			};
		}
		
		public static CpuCacheModel ToModel(this CpuCacheDto cpuCacheDto)
		{
			return new CpuCacheModel
			{
				Associativity = cpuCacheDto.Associativity,
				Name = cpuCacheDto.Name,
				Size = cpuCacheDto.Size,
				LineSize = cpuCacheDto.LineSize,
				LinesPerTag = cpuCacheDto.LinesPerTag
			};
		}
	}
}