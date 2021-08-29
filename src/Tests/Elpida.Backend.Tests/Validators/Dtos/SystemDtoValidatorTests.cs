using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class SystemDtoValidatorTests : ValidatorTest<SystemDto, SystemDtoValidator>
	{
		protected override IEnumerable<(SystemDto, string)> GetInvalidData()
		{
			yield return (
				new SystemDto(
					null!,
					Generators.NewOs(),
					Generators.NewTopology(),
					Generators.NewMemory(),
					Generators.NewTiming()
				), $"{nameof(SystemDto.Cpu)}");

			yield return (
				new SystemDto(
					Generators.NewCpu(),
					null!,
					Generators.NewTopology(),
					Generators.NewMemory(),
					Generators.NewTiming()
				), $"{nameof(SystemDto.Os)}");

			yield return (
				new SystemDto(
					Generators.NewCpu(),
					Generators.NewOs(),
					null!,
					Generators.NewMemory(),
					Generators.NewTiming()
				), $"{nameof(SystemDto.Topology)}");

			yield return (
				new SystemDto(
					Generators.NewCpu(),
					Generators.NewOs(),
					Generators.NewTopology(),
					null!,
					Generators.NewTiming()
				), $"{nameof(SystemDto.Memory)}");

			yield return (
				new SystemDto(
					Generators.NewCpu(),
					Generators.NewOs(),
					Generators.NewTopology(),
					Generators.NewMemory(),
					null!
				), $"{nameof(SystemDto.Timing)}");
		}
	}
}