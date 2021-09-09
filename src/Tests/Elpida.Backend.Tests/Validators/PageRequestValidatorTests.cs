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
	internal class PageRequestValidatorTests : ValidatorTest<PageRequest, PageRequestValidator>
	{
		protected override IEnumerable<(PageRequest, string)> GetInvalidData()
		{
			yield return (new PageRequest(-5, 10, 50), $"negative {nameof(PageRequest.Next)}");
			yield return (new PageRequest(5, -1, 50), $"negative {nameof(PageRequest.Count)}");
			yield return (new PageRequest(5, 0, 50), $"zero {nameof(PageRequest.Count)}");
			yield return (new PageRequest(5, PageRequest.MaxCount + 1, 50), $"very large {nameof(PageRequest.Count)}");
		}
	}
}