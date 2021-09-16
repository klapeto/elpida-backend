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

using Elpida.Backend.Controllers;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Tests;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Controllers
{
	[TestFixture]
	internal class ElpidaControllerTests : ServiceControllerTests<ElpidaVersionDto, ElpidaVersionDto, IElpidaVersionService>
	{
		protected override ServiceController<ElpidaVersionDto, ElpidaVersionDto, IElpidaVersionService> GetController(IElpidaVersionService versionService)
		{
			return new ElpidaVersionController(versionService);
		}

		protected override ElpidaVersionDto NewDummyDto()
		{
			return DtoGenerators.NewElpida();
		}

		protected override ElpidaVersionDto NewDummyPreviewDto()
		{
			return DtoGenerators.NewElpida();
		}
	}
}