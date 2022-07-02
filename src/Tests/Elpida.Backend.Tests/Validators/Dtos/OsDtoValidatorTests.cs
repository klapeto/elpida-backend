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
using Elpida.Backend.Services.Abstractions.Dtos.Os;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class OsDtoValidatorTests : ValidatorTest<OperatingSystemDto>
	{
		protected override IEnumerable<(OperatingSystemDto, string)> GetInvalidData()
		{
			yield return (new OperatingSystemDto(0, null!, "Test", "Test"), $"null {nameof(OperatingSystemDto.Category)}");
			yield return (new OperatingSystemDto(0, string.Empty, "Test", "Test"), $"empty {nameof(OperatingSystemDto.Category)}");
			yield return (new OperatingSystemDto(0, " ", "Test", "Test"), $"empty {nameof(OperatingSystemDto.Category)}");
			yield return (new OperatingSystemDto(0, new string('A', 100), "Test", "Test"), $"very large {nameof(OperatingSystemDto.Category)}");

			yield return (new OperatingSystemDto(0, "Test", null!, "Test"), $"null {nameof(OperatingSystemDto.Name)}");
			yield return (new OperatingSystemDto(0, "Test", string.Empty, "Test"), $"empty {nameof(OperatingSystemDto.Name)}");
			yield return (new OperatingSystemDto(0, "Test", " ", "Test"), $"empty {nameof(OperatingSystemDto.Name)}");
			yield return (new OperatingSystemDto(0, "Test", new string('A', 150), "Test"), $"very large {nameof(OperatingSystemDto.Name)}");

			yield return (new OperatingSystemDto(0, "Test", "Test", null!), $"null {nameof(OperatingSystemDto.Version)}");
			yield return (new OperatingSystemDto(0, "Test", "Test", string.Empty), $"empty {nameof(OperatingSystemDto.Version)}");
			yield return (new OperatingSystemDto(0, "Test", "Test", " "), $"empty {nameof(OperatingSystemDto.Version)}");
			yield return (new OperatingSystemDto(0, "Test", "Test", new string('A', 100)), $"very large {nameof(OperatingSystemDto.Version)}");
		}
	}
}