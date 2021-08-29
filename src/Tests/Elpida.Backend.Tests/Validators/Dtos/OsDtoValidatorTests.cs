using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class OsDtoValidatorTests : ValidatorTest<OsDto, OsDtoValidator>
	{
		protected override IEnumerable<(OsDto, string)> GetInvalidData()
		{
			yield return (new OsDto(5, "Test", "Test", "Test"), $"Non Zero {nameof(OsDto.Id)}");
			yield return (new OsDto(-5, "Test", "Test", "Test"), $"Non Zero {nameof(OsDto.Id)}");

			yield return (new OsDto(0, null!, "Test", "Test"), $"null {nameof(OsDto.Category)}");
			yield return (new OsDto(0, string.Empty, "Test", "Test"), $"empty {nameof(OsDto.Category)}");
			yield return (new OsDto(0, " ", "Test", "Test"), $"empty {nameof(OsDto.Category)}");
			yield return (new OsDto(0, new string('A', 100), "Test", "Test"), $"very large {nameof(OsDto.Category)}");

			yield return (new OsDto(0, "Test", null!, "Test"), $"null {nameof(OsDto.Name)}");
			yield return (new OsDto(0, "Test", string.Empty, "Test"), $"empty {nameof(OsDto.Name)}");
			yield return (new OsDto(0, "Test", " ", "Test"), $"empty {nameof(OsDto.Name)}");
			yield return (new OsDto(0, "Test", new string('A', 150), "Test"), $"very large {nameof(OsDto.Name)}");

			yield return (new OsDto(0, "Test", "Test", null!), $"null {nameof(OsDto.Version)}");
			yield return (new OsDto(0, "Test", "Test", string.Empty), $"empty {nameof(OsDto.Version)}");
			yield return (new OsDto(0, "Test", "Test", " "), $"empty {nameof(OsDto.Version)}");
			yield return (new OsDto(0, "Test", "Test", new string('A', 100)), $"very large {nameof(OsDto.Version)}");
		}
	}
}