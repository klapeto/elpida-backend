using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public static class ResultPreviewModelExtensions
	{
		public static ResultPreviewDto ToDto(this ResultPreviewModel model)
		{
			return new ResultPreviewDto
			{
				Name = model.Name,
				Id = model.Id,
				OsName = model.OsName,
				OsVersion = model.OsVersion,
				ElpidaVersionMajor = model.ElpidaVersionMajor,
				ElpidaVersionMinor = model.ElpidaVersionMinor,
				ElpidaVersionRevision = model.ElpidaVersionRevision,
				ElpidaVersionBuild = model.ElpidaVersionBuild,
				CpuBrand = model.CpuBrand,
				CpuCores = model.CpuCores,
				CpuLogicalCores = model.CpuLogicalCores,
				CpuFrequency = model.CpuFrequency,
				MemorySize = model.MemorySize,
				TimeStamp = model.TimeStamp
			};
		}
	}
}