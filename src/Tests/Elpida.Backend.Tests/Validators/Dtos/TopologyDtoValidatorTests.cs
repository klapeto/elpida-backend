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
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Tests;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class TopologyDtoValidatorTests : ValidatorTest<TopologyDto>
	{
		protected override IEnumerable<(TopologyDto, string)> GetInvalidData()
		{
			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				-5,
				10,
				10,
				10,
				DtoGenerators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalLogicalCores)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				-5,
				10,
				10,
				DtoGenerators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalPhysicalCores)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				10,
				-5,
				10,
				DtoGenerators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalNumaNodes)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				10,
				10,
				-5,
				DtoGenerators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalPackages)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				10,
				10,
				10,
				null!
			), $"null {nameof(TopologyDto.Root)}");
		}
	}
}