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
using System.Linq;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Controllers;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Tests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Controllers
{
	[TestFixture]
	internal class ResultControllerTests
		: ServiceControllerTests<ResultDto, ResultPreviewDto, IResultService>
	{
		[Test]
		public async Task PostNewResult_Success()
		{
			var service = new Mock<IResultService>(MockBehavior.Strict);
			var controller = new ResultController(service.Object);

			var batch = DtoGenerators.NewBenchmarkResultsBatch();

			var expectedResult = new List<long> { 8, 2, 3 };

			service.Setup(s => s.AddBatchAsync(batch, default))
				.ReturnsAsync(expectedResult);

			var result = await controller.PostNewResult(batch, default);

			Assert.True(result.GetType() == typeof(CreatedAtActionResult));

			var createdResult = (CreatedAtActionResult)result;
			Assert.AreEqual(nameof(ResultController.GetSingle), createdResult.ActionName);
			Assert.AreEqual(expectedResult.First(), createdResult.RouteValues.Values.First());

			service.Verify(s => s.AddBatchAsync(batch, default), Times.Once);
		}

		[Test]
		public void PostNewResult_ThrowsException_NoCatch()
		{
			var service = new Mock<IResultService>(MockBehavior.Strict);
			var controller = new ResultController(service.Object);

			var batch = DtoGenerators.NewBenchmarkResultsBatch();
			var expectedException = new NotFoundException("It was not found", 5);

			service.Setup(s => s.AddBatchAsync(batch, default))
				.Throws(expectedException);

			var actualException = Assert.ThrowsAsync<NotFoundException>(() => controller.PostNewResult(batch, default));

			Assert.AreEqual(expectedException, actualException);

			service.Verify(s => s.AddBatchAsync(batch, default), Times.Once);
		}

		protected override ServiceController<ResultDto, ResultPreviewDto, IResultService>
			GetController(IResultService service)
		{
			return new ResultController(service);
		}

		protected override ResultDto NewDummyDto()
		{
			return DtoGenerators.NewBenchmarkResult();
		}

		protected override ResultPreviewDto NewDummyPreviewDto()
		{
			return DtoGenerators.NewBenchmarkResultPreview();
		}
	}
}