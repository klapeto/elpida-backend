/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultServiceTests
	{
		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new ResultService(null));
		}


		[Test]
		public async Task CreateAsync_Success()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var id = Guid.NewGuid().ToString("N");

			repoMock.Setup(r =>
					r.CreateAsync(It.Is<ResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(id)
				.Verifiable();

			var service = new ResultService(repoMock.Object);

			var result = await service.CreateAsync(Generators.CreateNewResultDto(), default);

			Assert.AreEqual(id, result);

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public void CreateAsync_NullDto_ThrowsArgumentNullException()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var service = new ResultService(repoMock.Object);

			Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(null, default));

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetSingleAsync_Success()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var id = Guid.NewGuid().ToString("N");

			repoMock.Setup(r =>
					r.GetSingleAsync(It.Is<string>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => Generators.CreateNewResultModel(id))
				.Verifiable();

			var service = new ResultService(repoMock.Object);

			var result = await service.GetSingleAsync(id, default);

			Assert.AreEqual(id, result.Id);

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public void GetPagedAsync_NullPageRequest_ThrowsArgumentNullException()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var service = new ResultService(repoMock.Object);

			Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetPagedAsync(null, default));

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetPagedAsync_Success()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 100
			};

			repoMock.Setup(r =>
					r.GetAsync(It.Is<int>(i => i == page.Next), It.Is<int>(i => i == page.Count), true, default))
				.ReturnsAsync(() => new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				})
				.Verifiable();

			var service = new ResultService(repoMock.Object);

			var result = await service.GetPagedAsync(page, default);

			Assert.AreEqual(1, result.Count);

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetPagedAsync_NoTotalCount_FillsTotalCount()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 0
			};

			const int totalCount = 532;

			repoMock.Setup(r =>
					r.GetAsync(It.Is<int>(i => i == page.Next), It.Is<int>(i => i == page.Count), true, default))
				.ReturnsAsync(() => new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				})
				.Verifiable();

			repoMock.Setup(r =>
					r.GetTotalCountAsync(default))
				.ReturnsAsync(() => totalCount)
				.Verifiable();

			var service = new ResultService(repoMock.Object);

			var result = await service.GetPagedAsync(page, default);

			Assert.AreEqual(totalCount, result.TotalCount);

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  \t \n")]
		public void GetSingleAsync_InvalidId_ThrowsArgumentException(string id)
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var service = new ResultService(repoMock.Object);

			Assert.ThrowsAsync<ArgumentException>(async () => await service.GetSingleAsync(id, default));

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task DeleteAllAsync_Success()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			repoMock.Setup(r => r.DeleteAllAsync(default))
				.Returns(Task.CompletedTask)
				.Verifiable();

			var service = new ResultService(repoMock.Object);

			await service.ClearResultsAsync(default);

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}

		[Test]
		public void GetSingleAsync_NonExistent_ThrowsNotFoundException()
		{
			var repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);

			var id = Guid.NewGuid().ToString("N");

			repoMock.Setup(r =>
					r.GetSingleAsync(It.Is<string>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => null)
				.Verifiable();

			var service = new ResultService(repoMock.Object);

			Assert.ThrowsAsync<NotFoundException>(async () => await service.GetSingleAsync(id, default));

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}
	}
}