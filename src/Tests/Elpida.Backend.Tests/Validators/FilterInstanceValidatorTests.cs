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
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators
{
	internal class FilterInstanceValidatorTests : ValidatorTest<FilterInstance, FilterInstanceValidator>
	{
		protected override IEnumerable<(FilterInstance, string)> GetInvalidData()
		{
			yield return (new FilterInstance(null!, "Test", "eq"), $"null {nameof(FilterInstance.Name)}");
			yield return (new FilterInstance(string.Empty, "Test", "eq"), $"null {nameof(FilterInstance.Name)}");
			yield return (new FilterInstance(" ", "Test", "eq"), $"null {nameof(FilterInstance.Name)}");

			yield return (new FilterInstance("Test", null!, "Test"), $"null {nameof(FilterInstance.Value)}");

			yield return (new FilterInstance("Test", "Test", null!), $"null {nameof(FilterInstance.Comparison)}");
			yield return (new FilterInstance("Test", "Test", string.Empty),
				$"null {nameof(FilterInstance.Comparison)}");

			yield return (new FilterInstance("Test", "Test", " "), $"null {nameof(FilterInstance.Comparison)}");
		}
	}
}