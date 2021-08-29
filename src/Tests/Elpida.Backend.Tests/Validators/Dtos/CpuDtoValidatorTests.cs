using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class CpuDtoValidatorTests : ValidatorTest<CpuDto, CpuDtoValidator>
	{
		protected override IEnumerable<(CpuDto, string)> GetInvalidData()
		{
			yield return (new CpuDto(
				5,
				"Test",
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"Non zero {nameof(CpuDto.Id)}");

			yield return (new CpuDto(
				-1,
				"Test",
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"Non zero {nameof(CpuDto.Id)}");

			yield return (new CpuDto(
				0,
				null!,
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"null {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				string.Empty,
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				" ",
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				new string('A', 50),
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"very large {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				"Test",
				null!,
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"null {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				string.Empty,
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				" ",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				new string('A', 80),
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				null!,
				546,
				true,
				null,
				null,
				null
			), $"null {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				string.Empty,
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				" ",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				new string('A', 80),
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				"Test",
				-50,
				true,
				null,
				null,
				null
			), $"negative {nameof(CpuDto.Frequency)}");
		}
	}
}