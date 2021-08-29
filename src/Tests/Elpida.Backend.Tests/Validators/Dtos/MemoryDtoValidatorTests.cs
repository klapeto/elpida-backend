using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class MemoryDtoValidatorTests : ValidatorTest<MemoryDto, MemoryDtoValidator>
	{
		protected override IEnumerable<(MemoryDto, string)> GetInvalidData()
		{
			yield return (new MemoryDto(-50, 10), $"negative {nameof(MemoryDto.TotalSize)}");
			yield return (new MemoryDto(10, -50), $"negative {nameof(MemoryDto.PageSize)}");
		}
	}
}