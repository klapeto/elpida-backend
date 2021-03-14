using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class MemoryDataExtensions
	{
		public static MemoryDto ToDto(this MemoryModel model)
		{
			return new MemoryDto
			{
				PageSize = model.PageSize,
				TotalSize = model.TotalSize
			};
		}
		
		public static MemoryModel ToModel(this MemoryDto dto)
		{
			return new MemoryModel
			{
				PageSize = dto.PageSize,
				TotalSize = dto.TotalSize
			};
		}

	}
}