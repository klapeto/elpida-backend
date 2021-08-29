using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class CompilerDtoValidatorTests : ValidatorTest<CompilerDto, CompilerDtoValidator>
	{
		protected override IEnumerable<(CompilerDto, string)> GetInvalidData()
		{
			yield return (new CompilerDto(null!, "Test"), $"null {nameof(CompilerDto.Name)}");
			yield return (new CompilerDto(string.Empty, "Test"), $"empty {nameof(CompilerDto.Name)}");
			yield return (new CompilerDto(" ", "Test"), $"empty {nameof(CompilerDto.Name)}");
			yield return (new CompilerDto(new string('A', 150), "Test"), $"very large {nameof(CompilerDto.Name)}");

			yield return (new CompilerDto("Test", null!), $"null {nameof(CompilerDto.Version)}");
			yield return (new CompilerDto("Test", string.Empty), $"empty {nameof(CompilerDto.Version)}");
			yield return (new CompilerDto("Test", " "), $"empty {nameof(CompilerDto.Version)}");
			yield return (new CompilerDto("Test", new string('A', 150)), $"very large {nameof(CompilerDto.Version)}");
		}
	}
}