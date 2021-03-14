using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Cpu
{
	public static class CpuDataExtensions
	{
		public static CpuDto ToDto(this CpuModel model)
		{
			return new CpuDto
			{
				Brand = model.Brand,
				Caches = model.Caches.Select(c => c.ToDto()).ToList(),
				Features = model.Features,
				Frequency = model.Frequency,
				Smt = model.Smt,
				Vendor = model.Vendor,
				AdditionalInfo = model.AdditionalInfo
			};
		}
		
		public static CpuModel ToModel(this CpuDto cpuDto, string id)
		{
			return new CpuModel
			{
				Id = id,
				Brand = cpuDto.Brand,
				Caches = cpuDto.Caches.Select(c => c.ToModel()).ToList(),
				Features = cpuDto.Features,
				Frequency = cpuDto.Frequency,
				Smt = cpuDto.Smt,
				Vendor = cpuDto.Vendor,
				AdditionalInfo = cpuDto.AdditionalInfo
			};
		}
	}
}