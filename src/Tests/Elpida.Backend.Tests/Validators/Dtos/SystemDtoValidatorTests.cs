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
using Elpida.Backend.Services.Tests;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class SystemDtoValidatorTests : ValidatorTest<SystemDto>
	{
		protected override IEnumerable<(SystemDto, string)> GetInvalidData()
		{
			yield return (
				new SystemDto(
					null!,
					DtoGenerators.NewOs(),
					DtoGenerators.NewTopology(),
					DtoGenerators.NewMemory(),
					DtoGenerators.NewTiming()
				), $"{nameof(SystemDto.Cpu)}");

			yield return (
				new SystemDto(
					DtoGenerators.NewCpu(),
					null!,
					DtoGenerators.NewTopology(),
					DtoGenerators.NewMemory(),
					DtoGenerators.NewTiming()
				), $"{nameof(SystemDto.Os)}");

			yield return (
				new SystemDto(
					DtoGenerators.NewCpu(),
					DtoGenerators.NewOs(),
					null!,
					DtoGenerators.NewMemory(),
					DtoGenerators.NewTiming()
				), $"{nameof(SystemDto.Topology)}");

			yield return (
				new SystemDto(
					DtoGenerators.NewCpu(),
					DtoGenerators.NewOs(),
					DtoGenerators.NewTopology(),
					null!,
					DtoGenerators.NewTiming()
				), $"{nameof(SystemDto.Memory)}");

			yield return (
				new SystemDto(
					DtoGenerators.NewCpu(),
					DtoGenerators.NewOs(),
					DtoGenerators.NewTopology(),
					DtoGenerators.NewMemory(),
					null!
				), $"{nameof(SystemDto.Timing)}");
		}
	}
}