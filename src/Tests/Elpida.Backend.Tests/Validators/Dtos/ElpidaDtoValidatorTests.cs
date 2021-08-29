using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class ElpidaDtoValidatorTests : ValidatorTest<ElpidaDto, ElpidaDtoValidator>
	{
		protected override IEnumerable<(ElpidaDto, string)> GetInvalidData()
		{
			yield return (new ElpidaDto(5, Generators.NewVersion(), Generators.NewCompiler()),
				$"value in {nameof(ElpidaDto.Id)}");

			yield return (new ElpidaDto(0, null!, Generators.NewCompiler()),
				$"null {nameof(ElpidaDto.Version)}");

			yield return (new ElpidaDto(0, Generators.NewVersion(), null!),
				$"null {nameof(ElpidaDto.Compiler)}");
		}
	}
}