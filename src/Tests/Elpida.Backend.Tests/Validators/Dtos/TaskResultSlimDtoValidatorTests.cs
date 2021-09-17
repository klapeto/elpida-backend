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

using System;
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class TaskResultSlimDtoValidatorTests : ValidatorTest<TaskResultSlimDto>
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