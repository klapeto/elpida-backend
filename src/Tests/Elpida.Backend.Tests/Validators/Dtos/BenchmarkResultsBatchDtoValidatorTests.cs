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
using Elpida.Backend.Services.Tests;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class BenchmarkResultsBatchDtoValidatorTests
		: ValidatorTest<BenchmarkResultsBatchDto, BenchmarkResultsBatchDtoValidator>
	{
		protected override IEnumerable<(BenchmarkResultsBatchDto, string)> GetInvalidData()
		{
			yield return (
				new BenchmarkResultsBatchDto(
					5,
					DtoGenerators.NewElpida(),
					DtoGenerators.NewSystem(),
					new BenchmarkResultSlimDto[] { null! }
				), $"Non zero {nameof(BenchmarkResultsBatchDto.Id)}");

			yield return (
				new BenchmarkResultsBatchDto(
					-5,
					DtoGenerators.NewElpida(),
					DtoGenerators.NewSystem(),
					new BenchmarkResultSlimDto[] { null! }
				), $"Non zero {nameof(BenchmarkResultsBatchDto.Id)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					null!,
					DtoGenerators.NewSystem(),
					new BenchmarkResultSlimDto[] { null! }
				), $"null {nameof(BenchmarkResultsBatchDto.Elpida)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					DtoGenerators.NewElpida(),
					null!,
					new BenchmarkResultSlimDto[] { null! }
				), $"null {nameof(BenchmarkResultsBatchDto.System)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					DtoGenerators.NewElpida(),
					DtoGenerators.NewSystem(),
					null!
				), $"null {nameof(BenchmarkResultsBatchDto.BenchmarkResults)}");

			yield return (
				new BenchmarkResultsBatchDto(
					0,
					DtoGenerators.NewElpida(),
					DtoGenerators.NewSystem(),
					Array.Empty<BenchmarkResultSlimDto>()
				), $"empty {nameof(BenchmarkResultsBatchDto.BenchmarkResults)}");
		}
	}
}