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
using Elpida.Backend.Services.Abstractions.Dtos.Statistics;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Tests;

namespace Elpida.Backend.Tests.Controllers
{
	internal class BenchmarkStatisticsControllerTests
		: ServiceControllerTests<BenchmarkStatisticsDto, BenchmarkStatisticsPreviewDto, IBenchmarkStatisticsService>
	{
		protected override
			ServiceController<BenchmarkStatisticsDto, BenchmarkStatisticsPreviewDto, IBenchmarkStatisticsService>
			GetController(IBenchmarkStatisticsService service)
		{
			return new BenchmarkStatisticsController(service);
		}

		protected override BenchmarkStatisticsDto NewDummyDto()
		{
			return DtoGenerators.NewBenchmarkStatistic();
		}

		protected override BenchmarkStatisticsPreviewDto NewDummyPreviewDto()
		{
			return DtoGenerators.NewBenchmarkStatisticsPreview();
		}
	}
}