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
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Tests;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Controllers
{
	[TestFixture]
	internal class BenchmarkControllerTests : ServiceControllerTests<BenchmarkDto, BenchmarkPreviewDto, IBenchmarkService>
	{
		protected override ServiceController<BenchmarkDto, BenchmarkPreviewDto, IBenchmarkService> GetController(
			IBenchmarkService service
		)
		{
			return new BenchmarkController(service);
		}

		protected override BenchmarkDto NewDummyDto()
		{
			return DtoGenerators.NewBenchmark();
		}

		protected override BenchmarkPreviewDto NewDummyPreviewDto()
		{
			return DtoGenerators.NewBenchmarkPreview();
		}
	}
}