using System;
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class TaskResultSlimDtoValidatorTests : ValidatorTest<TaskResultSlimDto, TaskResultSlimDtoValidator>
	{
		protected override IEnumerable<(TaskResultSlimDto, string)> GetInvalidData()
		{
			yield return (new TaskResultSlimDto(
				Guid.Empty,
				51,
				156,
				654,
				new TaskRunStatisticsDto(54, 654, 13, 64, 89, 64, 12)
			), $"empty {nameof(TaskResultSlimDto.Uuid)}");

			yield return (new TaskResultSlimDto(
				Guid.NewGuid(),
				0,
				156,
				654,
				new TaskRunStatisticsDto(54, 654, 13, 64, 89, 64, 12)
			), $"zero {nameof(TaskResultSlimDto.Value)}");

			yield return (new TaskResultSlimDto(
				Guid.NewGuid(),
				-5,
				156,
				654,
				new TaskRunStatisticsDto(54, 654, 13, 64, 89, 64, 12)
			), $"negative {nameof(TaskResultSlimDto.Value)}");

			yield return (new TaskResultSlimDto(
				Guid.NewGuid(),
				64,
				0,
				654,
				new TaskRunStatisticsDto(54, 654, 13, 64, 89, 64, 12)
			), $"zero {nameof(TaskResultSlimDto.Time)}");

			yield return (new TaskResultSlimDto(
				Guid.NewGuid(),
				0,
				-1.0,
				654,
				new TaskRunStatisticsDto(54, 654, 13, 64, 89, 64, 12)
			), $"negative {nameof(TaskResultSlimDto.Time)}");

			yield return (new TaskResultSlimDto(
				Guid.NewGuid(),
				0,
				-1.0,
				654,
				null!
			), $"null {nameof(TaskResultSlimDto.Statistics)}");
		}
	}
}