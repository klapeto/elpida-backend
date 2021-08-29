using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class CpuCacheDtoValidatorTests : ValidatorTest<CpuCacheDto, CpuCacheDtoValidator>
	{
		protected override IEnumerable<(CpuCacheDto, string)> GetInvalidData()
		{
			yield return (new CpuCacheDto(null!, "Test", 456, 465), $"null {nameof(CpuCacheDto.Name)}");
			yield return (new CpuCacheDto(string.Empty, "Test", 456, 465), $"empty {nameof(CpuCacheDto.Name)}");
			yield return (new CpuCacheDto(" ", "Test", 456, 465), $"empty {nameof(CpuCacheDto.Name)}");
			yield return (new CpuCacheDto(new string('A', 80), "Test", 456, 465),
				$"very large {nameof(CpuCacheDto.Name)}");

			yield return (new CpuCacheDto("Test", null!, 456, 465), $"null {nameof(CpuCacheDto.Associativity)}");
			yield return (new CpuCacheDto("Test", string.Empty, 456, 465),
				$"very large {nameof(CpuCacheDto.Associativity)}");
			yield return (new CpuCacheDto("Test", " ", 456, 465),
				$"very large {nameof(CpuCacheDto.Associativity)}");
			yield return (new CpuCacheDto("Test", new string('A', 80), 456, 465),
				$"empty {nameof(CpuCacheDto.Associativity)}");

			yield return (new CpuCacheDto("Test", "Test", -5, 465), $"negative {nameof(CpuCacheDto.Size)}");
			yield return (new CpuCacheDto("Test", "Test", 654, -8), $"negative {nameof(CpuCacheDto.LineSize)}");
		}
	}
}