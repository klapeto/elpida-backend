using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class VersionDataExtensions
	{
		public static VersionDto ToDto(this VersionModel model)
		{
			return new VersionDto
			{
				Build = model.Build,
				Major = model.Major,
				Minor = model.Minor,
				Revision = model.Revision
			};
		}
		
		public static VersionModel ToModel(this VersionDto dto)
		{
			return new VersionModel
			{
				Build = dto.Build,
				Major = dto.Major,
				Minor = dto.Minor,
				Revision = dto.Revision
			};
		}

	}
}