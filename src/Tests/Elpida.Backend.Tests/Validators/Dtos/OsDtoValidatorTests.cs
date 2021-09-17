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
	internal class OsDtoValidatorTests : ValidatorTest<OsDto>
	{
		protected override IEnumerable<(OsDto, string)> GetInvalidData()
		{
			yield return (new OsDto(0, null!, "Test", "Test"), $"null {nameof(OsDto.Category)}");
			yield return (new OsDto(0, string.Empty, "Test", "Test"), $"empty {nameof(OsDto.Category)}");
			yield return (new OsDto(0, " ", "Test", "Test"), $"empty {nameof(OsDto.Category)}");
			yield return (new OsDto(0, new string('A', 100), "Test", "Test"), $"very large {nameof(OsDto.Category)}");

			yield return (new OsDto(0, "Test", null!, "Test"), $"null {nameof(OsDto.Name)}");
			yield return (new OsDto(0, "Test", string.Empty, "Test"), $"empty {nameof(OsDto.Name)}");
			yield return (new OsDto(0, "Test", " ", "Test"), $"empty {nameof(OsDto.Name)}");
			yield return (new OsDto(0, "Test", new string('A', 150), "Test"), $"very large {nameof(OsDto.Name)}");

			yield return (new OsDto(0, "Test", "Test", null!), $"null {nameof(OsDto.Version)}");
			yield return (new OsDto(0, "Test", "Test", string.Empty), $"empty {nameof(OsDto.Version)}");
			yield return (new OsDto(0, "Test", "Test", " "), $"empty {nameof(OsDto.Version)}");
			yield return (new OsDto(0, "Test", "Test", new string('A', 100)), $"very large {nameof(OsDto.Version)}");
		}
	}
}