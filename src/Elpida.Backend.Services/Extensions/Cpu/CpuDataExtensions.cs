using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Cpu
{
	public static class CpuDataExtensions
	{
		public static CpuDto ToDto(this CpuModel model)
		{
			return new CpuDto
			{
				Brand = model.Brand,
				Caches = JsonConvert.DeserializeObject<List<CpuCacheDto>>(model.Caches),
				Features = JsonConvert.DeserializeObject<List<string>>(model.Features),
				Frequency = model.Frequency,
				Smt = model.Smt,
				Vendor = model.Vendor,
				AdditionalInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.AdditionalInfo)
			};
		}
		
		public static CpuModel ToModel(this CpuDto cpuDto, int id)
		{
			return new CpuModel
			{
				Id = id,
				Brand = cpuDto.Brand,
				Caches = JsonConvert.SerializeObject(cpuDto.Caches),
				Features = JsonConvert.SerializeObject(cpuDto.Features),
				Frequency = cpuDto.Frequency,
				Smt = cpuDto.Smt,
				Vendor = cpuDto.Vendor,
				AdditionalInfo = JsonConvert.SerializeObject(cpuDto.AdditionalInfo)
			};
		}
	}
}