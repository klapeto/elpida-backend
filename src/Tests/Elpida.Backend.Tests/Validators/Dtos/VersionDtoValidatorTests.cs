using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class VersionDtoValidatorTests : ValidatorTest<VersionDto, VersionDtoValidator>
	{
		protected override IEnumerable<(VersionDto, string)> GetInvalidData()
		{
			yield return (new VersionDto(-1, 0, 0, 0), $"negative {nameof(VersionDto.Major)}");
			yield return (new VersionDto(0, -1, 0, 0), $"negative {nameof(VersionDto.Minor)}");
			yield return (new VersionDto(0, 0, -1, 0), $"negative {nameof(VersionDto.Revision)}");
			yield return (new VersionDto(0, 0, 0, -1), $"negative {nameof(VersionDto.Build)}");
		}
	}
}