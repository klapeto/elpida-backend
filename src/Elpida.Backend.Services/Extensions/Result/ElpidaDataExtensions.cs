using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class ElpidaDataExtensions
	{
		public static ElpidaDto ToDto(this ElpidaModel model)
		{
			return new ElpidaDto
			{
				Compiler = model.Compiler.ToDto(),
				Version = model.Version.ToDto()
			};
		}
		
		public static ElpidaModel ToModel(this ElpidaDto dto)
		{
			return new ElpidaModel
			{
				Compiler = dto.Compiler.ToModel(),
				Version = dto.Version.ToModel()
			};
		}
	}
}