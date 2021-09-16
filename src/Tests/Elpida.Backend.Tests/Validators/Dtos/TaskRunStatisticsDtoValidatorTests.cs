// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class TaskRunStatisticsDtoValidatorTests : ValidatorTest<TaskRunStatisticsDto, TaskRunStatisticsDtoValidator>
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