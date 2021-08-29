using System;
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class BenchmarkResultsBatchDtoValidatorTests
		: ValidatorTest<BenchmarkResultsBatchDto, BenchmarkResultsBatchDtoValidator>
	{
		protected override IEnumerable<(BenchmarkResultsBatchDto, string)> GetInvalidData()
		{
			yield return (
				new BenchmarkResultsBatchDto(
					5,
					Generators.NewElpida(),
					Generators.NewSystem(),
					new BenchmarkResultSlimDto[] { null! }
				), $"Non zero {nameof(BenchmarkResultsBatchDto.Id)}");

			yield return (
				new BenchmarkResultsBatchDto(
					-5,
					Generators.NewElpida(),
					Generators.NewSystem(),
					new BenchmarkResultSlimDto[] { null! }
				), $"Non zero {nameof(BenchmarkResultsBatchDto.Id)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					null!,
					Generators.NewSystem(),
					new BenchmarkResultSlimDto[] { null! }
				), $"null {nameof(BenchmarkResultsBatchDto.Elpida)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					Generators.NewElpida(),
					null!,
					new BenchmarkResultSlimDto[] { null! }
				), $"null {nameof(BenchmarkResultsBatchDto.System)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					Generators.NewElpida(),
					Generators.NewSystem(),
					null!
				), $"null {nameof(BenchmarkResultsBatchDto.BenchmarkResults)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					Generators.NewElpida(),
					Generators.NewSystem(),
					Array.Empty<BenchmarkResultSlimDto>()
				), $"empty {nameof(BenchmarkResultsBatchDto.BenchmarkResults)}");
		}
	}
}