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
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class CpuDtoValidatorTests : ValidatorTest<CpuDto, CpuDtoValidator>
	{
		protected override IEnumerable<(CpuDto, string)> GetInvalidData()
		{
			yield return (new CpuDto(
				5,
				"Test",
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"Non zero {nameof(CpuDto.Id)}");

			yield return (new CpuDto(
				-1,
				"Test",
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"Non zero {nameof(CpuDto.Id)}");

			yield return (new CpuDto(
				0,
				null!,
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"null {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				string.Empty,
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				" ",
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				new string('A', 50),
				"Test",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"very large {nameof(CpuDto.Architecture)}");

			yield return (new CpuDto(
				0,
				"Test",
				null!,
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"null {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				string.Empty,
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				" ",
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				new string('A', 80),
				"Test",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.Vendor)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				null!,
				546,
				true,
				null,
				null,
				null
			), $"null {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				string.Empty,
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				" ",
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				new string('A', 80),
				546,
				true,
				null,
				null,
				null
			), $"empty {nameof(CpuDto.ModelName)}");

			yield return (new CpuDto(
				0,
				"Test",
				"Test",
				"Test",
				-50,
				true,
				null,
				null,
				null
			), $"negative {nameof(CpuDto.Frequency)}");
		}
	}
}