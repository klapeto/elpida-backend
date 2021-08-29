using System;
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Validators;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	[TestFixture]
	public class BenchmarkResultSlimValidatorTests : ValidatorTest<BenchmarkResultSlimDto, BenchmarkResultSlimDtoValidator>
	{
		protected override IEnumerable<(BenchmarkResultSlimDto, string)> GetInvalidData()
		{
			yield return (new BenchmarkResultSlimDto(
				default,
				DateTime.Now,
				new long[] { 1, 2, 3 },
				5,
				new TaskResultSlimDto[] { null! }
			), $"default {nameof(BenchmarkResultSlimDto.Uuid)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				default,
				new long[] { 1, 2, 3 },
				5,
				new TaskResultSlimDto[] { null! }
			), $"default {nameof(BenchmarkResultSlimDto.Timestamp)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				default!,
				5,
				new TaskResultSlimDto[] { null! }
			), $"default {nameof(BenchmarkResultSlimDto.Affinity)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				Array.Empty<long>(),
				5,
				new TaskResultSlimDto[] { null! }
			), $"empty {nameof(BenchmarkResultSlimDto.Affinity)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				new long[] { 1, 2, 3 },
				default,
				Array.Empty<TaskResultSlimDto>()
			), $"empty {nameof(BenchmarkResultSlimDto.TaskResults)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				new long[] { 1, 2, 3 },
				5,
				default!
			), $"default {nameof(BenchmarkResultSlimDto.TaskResults)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				new long[] { 1, 2, 3 },
				-5.0,
				new TaskResultSlimDto[] { null! }
			), $"negative {nameof(BenchmarkResultSlimDto.Score)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				new long[] { 1, 2, 3 },
				0.0,
				new TaskResultSlimDto[] { null! }
			), $"zero {nameof(BenchmarkResultSlimDto.Score)}");

			yield return (new BenchmarkResultSlimDto(
				Guid.NewGuid(),
				DateTime.Now,
				new long[] { 1, -2, 3 },
				-5.0,
				new TaskResultSlimDto[] { null! }
			), $"negative {nameof(BenchmarkResultSlimDto.Affinity)}");
		}
	}
}