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
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class CompilerDtoValidatorTests : ValidatorTest<CompilerDto>
	{
		protected override IEnumerable<(CompilerDto, string)> GetInvalidData()
		{
			yield return (new CompilerDto(null!, "Test"), $"null {nameof(CompilerDto.Name)}");
			yield return (new CompilerDto(string.Empty, "Test"), $"empty {nameof(CompilerDto.Name)}");
			yield return (new CompilerDto(" ", "Test"), $"empty {nameof(CompilerDto.Name)}");
			yield return (new CompilerDto(new string('A', 150), "Test"), $"very large {nameof(CompilerDto.Name)}");

			yield return (new CompilerDto("Test", null!), $"null {nameof(CompilerDto.Version)}");
			yield return (new CompilerDto("Test", string.Empty), $"empty {nameof(CompilerDto.Version)}");
			yield return (new CompilerDto("Test", " "), $"empty {nameof(CompilerDto.Version)}");
			yield return (new CompilerDto("Test", new string('A', 150)), $"very large {nameof(CompilerDto.Version)}");
		}
	}
}