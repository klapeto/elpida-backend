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
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Validators;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	[TestFixture]
	internal class BenchmarkResultSlimValidatorTests : ValidatorTest<BenchmarkResultSlimDto, BenchmarkResultSlimDtoValidator>
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