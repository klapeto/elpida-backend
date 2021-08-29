using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class TaskRunStatisticsDtoValidatorTests : ValidatorTest<TaskRunStatisticsDto, TaskRunStatisticsDtoValidator>
	{
		protected override IEnumerable<(TaskRunStatisticsDto, string)> GetInvalidData()
		{
			yield return (new TaskRunStatisticsDto(0, 31, 654, 46, 13, 12, 97),
				$"zero {nameof(TaskRunStatisticsDto.SampleSize)}");

			yield return (new TaskRunStatisticsDto(-15, 31, 654, 46, 13, 12, 97),
				$"zero {nameof(TaskRunStatisticsDto.SampleSize)}");

			yield return (new TaskRunStatisticsDto(1, -5, 654, 46, 13, 12, 97),
				$"negative {nameof(TaskRunStatisticsDto.Max)}");

			yield return (new TaskRunStatisticsDto(1, 56, -15, 46, 13, 12, 97),
				$"negative {nameof(TaskRunStatisticsDto.Min)}");

			yield return (new TaskRunStatisticsDto(1, 56, 654, -5, 13, 12, 97),
				$"negative {nameof(TaskRunStatisticsDto.Mean)}");

			yield return (new TaskRunStatisticsDto(1, 35, 654, 46, -4, 12, 97),
				$"negative {nameof(TaskRunStatisticsDto.StandardDeviation)}");

			yield return (new TaskRunStatisticsDto(1, 35, 654, 46, 46, -5, 97),
				$"negative {nameof(TaskRunStatisticsDto.Tau)}");

			yield return (new TaskRunStatisticsDto(1, 35, 654, 46, 46, 5, -54),
				$"negative {nameof(TaskRunStatisticsDto.MarginOfError)}");
		}
	}
}