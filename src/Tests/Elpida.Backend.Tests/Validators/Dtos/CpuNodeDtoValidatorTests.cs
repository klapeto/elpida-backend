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
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class CpuNodeDtoValidatorTests : ValidatorTest<CpuNodeDto, CpuNodeDtoValidator>
	{
		protected override IEnumerable<(CpuNodeDto, string)> GetInvalidData()
		{
			yield return (new CpuNodeDto(
					(ProcessorNodeType)(-5),
					"Test",
					5,
					1,
					null,
					null
				),
				$"negative {nameof(CpuNodeDto.NodeType)}");

			yield return (new CpuNodeDto(
					(ProcessorNodeType)5000,
					"Test",
					5,
					1,
					null,
					null
				),
				$"invalid {nameof(CpuNodeDto.NodeType)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					null!,
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					string.Empty,
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					" ",
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					new string('A', 100),
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");
		}
	}
}