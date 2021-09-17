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

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class CpuCacheDtoValidatorTests : ValidatorTest<CpuCacheDto>
	{
		protected override IEnumerable<(CpuCacheDto, string)> GetInvalidData()
		{
			yield return (new CpuCacheDto(null!, "Test", 456, 465), $"null {nameof(CpuCacheDto.Name)}");
			yield return (new CpuCacheDto(string.Empty, "Test", 456, 465), $"empty {nameof(CpuCacheDto.Name)}");
			yield return (new CpuCacheDto(" ", "Test", 456, 465), $"empty {nameof(CpuCacheDto.Name)}");
			yield return (new CpuCacheDto(new string('A', 80), "Test", 456, 465),
				$"very large {nameof(CpuCacheDto.Name)}");

			yield return (new CpuCacheDto("Test", null!, 456, 465), $"null {nameof(CpuCacheDto.Associativity)}");
			yield return (new CpuCacheDto("Test", string.Empty, 456, 465),
				$"very large {nameof(CpuCacheDto.Associativity)}");

			yield return (new CpuCacheDto("Test", " ", 456, 465),
				$"very large {nameof(CpuCacheDto.Associativity)}");

			yield return (new CpuCacheDto("Test", new string('A', 80), 456, 465),
				$"empty {nameof(CpuCacheDto.Associativity)}");

			yield return (new CpuCacheDto("Test", "Test", -5, 465), $"negative {nameof(CpuCacheDto.Size)}");
			yield return (new CpuCacheDto("Test", "Test", 654, -8), $"negative {nameof(CpuCacheDto.LineSize)}");
		}
	}
}