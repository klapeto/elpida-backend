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
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultServiceTests
	{
		private Mock<IResultsRepository> _repoMock;
		private Mock<ICpuRepository> _cpuMock;
		private Mock<ITopologyRepository> _topologyMock;
		private Mock<IIdProvider> _idProvideMock;


		[SetUp]
		public void CreateMocks()
		{
			_repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);
			_cpuMock = new Mock<ICpuRepository>(MockBehavior.Strict);
			_topologyMock = new Mock<ITopologyRepository>(MockBehavior.Strict);
			_idProvideMock = new Mock<IIdProvider>(MockBehavior.Strict);
		}

		private ResultService GetService()
		{
			return new ResultService(_repoMock.Object, _cpuMock.Object, _topologyMock.Object, _idProvideMock.Object);
		}

		private void VerifyMocks()
		{
			_repoMock.VerifyAll();
			_repoMock.VerifyNoOtherCalls();
			_cpuMock.VerifyAll();
			_cpuMock.VerifyNoOtherCalls();
			_topologyMock.VerifyAll();
			_topologyMock.VerifyNoOtherCalls();
			_idProvideMock.VerifyAll();
			_idProvideMock.VerifyNoOtherCalls();
		}

		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new ResultService(null,
				Mock.Of<ICpuRepository>(),
				Mock.Of<ITopologyRepository>(),
				Mock.Of<IIdProvider>()));

			Assert.Throws<ArgumentNullException>(() => new ResultService(null,
				Mock.Of<ICpuRepository>(),
				Mock.Of<ITopologyRepository>(),
				Mock.Of<IIdProvider>()));

			Assert.Throws<ArgumentNullException>(() => new ResultService(Mock.Of<IResultsRepository>(),
				null,
				Mock.Of<ITopologyRepository>(),
				Mock.Of<IIdProvider>()));

			Assert.Throws<ArgumentNullException>(() => new ResultService(null,
				Mock.Of<ICpuRepository>(),
				null,
				Mock.Of<IIdProvider>()));

			Assert.Throws<ArgumentNullException>(() => new ResultService(null,
				Mock.Of<ICpuRepository>(),
				Mock.Of<ITopologyRepository>(),
				null));
		}
		
		[Test]
		public async Task CreateAsync_CpuNotExist_CreatesCpu()
		{
			var resultId = Guid.NewGuid().ToString("N");
			var cpuId = Guid.NewGuid().ToString("N");
			var topologyId = Guid.NewGuid().ToString("N");

			_repoMock.Setup(r =>
					r.CreateAsync(It.Is<ResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(resultId)
				.Verifiable();

			_idProvideMock.Setup(i => i.GetForCpu(It.IsAny<CpuDto>()))
				.Returns(cpuId)
				.Verifiable();

			_idProvideMock.Setup(i => i.GetForTopology(cpuId,It.IsAny<TopologyDto>()))
				.Returns(topologyId)
				.Verifiable();
			
			_idProvideMock.Setup(i => i.GetForResult(It.IsAny<ResultDto>()))
				.Returns(resultId)
				.Verifiable();

			_cpuMock.Setup(i => i.GetSingleAsync(cpuId, default))
				.ReturnsAsync((CpuModel)null)
				.Verifiable();
			
			_cpuMock.Setup(i => i.CreateAsync(It.Is<CpuModel>(c => c.Id == cpuId), default))
				.ReturnsAsync(cpuId)
				.Verifiable();

			_topologyMock.Setup(i => i.GetSingleAsync(topologyId, default))
				.ReturnsAsync(Generators.CreateNewTopology)
				.Verifiable();
			
			var service = GetService();

			var result = await service.CreateAsync(Generators.CreateNewResultDto(), default);

			Assert.AreEqual(resultId, result);

			VerifyMocks();
		}
		
		[Test]
		public async Task CreateAsync_TopologyNotExist_CreatesTopology()
		{
			var id = Guid.NewGuid().ToString("N");
			var cpuId = Guid.NewGuid().ToString("N");
			var topologyId = Guid.NewGuid().ToString("N");

			_repoMock.Setup(r =>
					r.CreateAsync(It.Is<ResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(id)
				.Verifiable();

			_idProvideMock.Setup(i => i.GetForCpu(It.IsAny<CpuDto>()))
				.Returns(cpuId)
				.Verifiable();

			_idProvideMock.Setup(i => i.GetForTopology(cpuId,It.IsAny<TopologyDto>()))
				.Returns(topologyId)
				.Verifiable();
			
			_idProvideMock.Setup(i => i.GetForResult(It.IsAny<ResultDto>()))
				.Returns(id)
				.Verifiable();

			_cpuMock.Setup(i => i.GetSingleAsync(cpuId, default))
				.ReturnsAsync(Generators.CreateNewCpuModel)
				.Verifiable();
			
			_topologyMock.Setup(i => i.GetSingleAsync(topologyId, default))
				.ReturnsAsync((TopologyModel)null)
				.Verifiable();
			
			_topologyMock.Setup(i => i.CreateAsync(It.Is<TopologyModel>(c => c.Id == topologyId), default))
				.ReturnsAsync(topologyId)
				.Verifiable();

			
			var service = GetService();

			var result = await service.CreateAsync(Generators.CreateNewResultDto(), default);

			Assert.AreEqual(id, result);

			VerifyMocks();
		}
		
		[Test]
		public async Task CreateAsync_Success()
		{
			var id = Guid.NewGuid().ToString("N");
			var cpuId = Guid.NewGuid().ToString("N");
			var topologyId = Guid.NewGuid().ToString("N");

			_repoMock.Setup(r =>
					r.CreateAsync(It.Is<ResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(id)
				.Verifiable();

			_idProvideMock.Setup(i => i.GetForCpu(It.IsAny<CpuDto>()))
				.Returns(cpuId)
				.Verifiable();

			_idProvideMock.Setup(i => i.GetForTopology(cpuId,It.IsAny<TopologyDto>()))
				.Returns(topologyId)
				.Verifiable();
			
			_idProvideMock.Setup(i => i.GetForResult(It.IsAny<ResultDto>()))
				.Returns(id)
				.Verifiable();

			_cpuMock.Setup(i => i.GetSingleAsync(cpuId, default))
				.ReturnsAsync(Generators.CreateNewCpuModel)
				.Verifiable();
			
			_topologyMock.Setup(i => i.GetSingleAsync(topologyId, default))
				.ReturnsAsync(Generators.CreateNewTopology)
				.Verifiable();
			
			var service = GetService();

			var result = await service.CreateAsync(Generators.CreateNewResultDto(), default);

			Assert.AreEqual(id, result);

			VerifyMocks();
		}

		[Test]
		public void CreateAsync_NullDto_ThrowsArgumentNullException()
		{
			var service = GetService();

			Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(null, default));

			VerifyMocks();
		}

		[Test]
		public async Task GetSingleAsync_Success()
		{
			var id = Guid.NewGuid().ToString("N");

			_repoMock.Setup(r =>
					r.GetProjectionAsync(It.Is<string>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => Generators.CreateProjection(id))
				.Verifiable();

			var service = GetService();

			var result = await service.GetSingleAsync(id, default);

			Assert.AreEqual(id, result.Id);

			VerifyMocks();
		}

		[Test]
		public void  GetSingleAsync_NonExist_ThrowsNotFoundException()
		{
			var id = Guid.NewGuid().ToString("N");

			_repoMock.Setup(r =>
					r.GetProjectionAsync(It.Is<string>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => null)
				.Verifiable();

			var service = GetService();

			Assert.ThrowsAsync<NotFoundException>(async () => await service.GetSingleAsync(id, default));

			VerifyMocks();
		}

		[Test]
		public void GetPagedAsync_NullPageRequest_ThrowsArgumentNullException()
		{
			var service = GetService();

			Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetPagedAsync(null, default));

			VerifyMocks();
		}

		[Test]
		public async Task GetPagedAsync_Success()
		{
			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 100
			};

			_repoMock.Setup(r =>
					r.GetPagedPreviewsAsync<object>(It.Is<int>(i => i == page.Next), 
						It.Is<int>(i => i == page.Count),
						false, 
						null,
						It.Is<IEnumerable<Expression<Func<ResultProjection, bool>>>>(e => !e.Any()), 
						false, 
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(0, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				}))
				.Verifiable();

			var service = GetService();

			var result = await service.GetPagedAsync(new QueryRequest
			{
				PageRequest = page
			}, default);

			Assert.AreEqual(1, result.Count);

			VerifyMocks();
		}

		[Test]
		public async Task GetPagedAsync_NoTotalCount_FillsTotalCount()
		{
			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 0
			};

			const int totalCount = 532;

			_repoMock.Setup(r =>
					r.GetPagedPreviewsAsync<object>(
						It.Is<int>(i => i == page.Next), 
						It.Is<int>(i => i == page.Count), 
						false,
						null,
						It.Is<IEnumerable<Expression<Func<ResultProjection, bool>>>>(e => !e.Any()), 
						true, 
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				}))
				.Verifiable();

			var service = GetService();

			var result = await service.GetPagedAsync(new QueryRequest
			{
				PageRequest = page
			}, default);

			Assert.AreEqual(totalCount, result.TotalCount);

			VerifyMocks();
		}

		[Test]
		public async Task GetPagedAsync_AppliesFilters()
		{
			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 0
			};

			const int totalCount = 532;
			const string filterValue = "Crap";

			_repoMock.Setup(r =>
					r.GetPagedPreviewsAsync<object>(
						It.Is<int>(i => i == page.Next), 
						It.Is<int>(i => i == page.Count), 
						false,
						null,
						It.Is<IEnumerable<Expression<Func<ResultProjection, bool>>>>(enumerable =>
							enumerable.Any(x => x.Body.ToString().Contains(filterValue))), 
						true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				}))
				.Verifiable();

			var service = GetService();

			var result = await service.GetPagedAsync(new QueryRequest
			{
				PageRequest = page,
				Filters = new[]
				{
					new QueryInstance
					{
						Name = "name",
						Comp = "eq",
						Value = filterValue
					}
				}
			}, default);

			Assert.AreEqual(totalCount, result.TotalCount);

			VerifyMocks();
		}

		[Test]
		public async Task GetPagedAsync_AppliesContains()
		{
			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 0
			};

			const int totalCount = 532;
			const string filterValue = "Crap";

			_repoMock.Setup(r =>
					r.GetPagedPreviewsAsync<object>(
						It.Is<int>(i => i == page.Next), 
						It.Is<int>(i => i == page.Count), 
						false, 
						null,
						It.Is<IEnumerable<Expression<Func<ResultProjection, bool>>>>(enumerable =>
							enumerable.Any(x => x.Body.ToString().Contains(filterValue))), 
						true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				}))
				.Verifiable();

			var service = GetService();

			var result = await service.GetPagedAsync(new QueryRequest
			{
				PageRequest = page,
				Filters = new[]
				{
					new QueryInstance
					{
						Name = "name",
						Comp = "c",
						Value = filterValue
					}
				}
			}, default);

			Assert.AreEqual(totalCount, result.TotalCount);

			VerifyMocks();
		}

		[Test]
		[TestCase("eq", "==")]
		[TestCase("ge", ">=")]
		[TestCase("g", ">")]
		[TestCase("le", "<=")]
		[TestCase("l", "<")]
		public async Task GetPagedAsync_Applies_Checks_Literals(string equalityType, string stringToCheck)
		{
			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 0
			};

			const int totalCount = 532;

			_repoMock.Setup(r =>
					r.GetPagedPreviewsAsync<object>(
						It.Is<int>(i => i == page.Next), 
						It.Is<int>(i => i == page.Count), 
						false, 
						null,
						It.Is<IEnumerable<Expression<Func<ResultProjection, bool>>>>(enumerable =>
							enumerable.Any(x => x.Body.ToString().Contains(stringToCheck))), true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				}))
				.Verifiable();

			var service = GetService();

			var result = await service.GetPagedAsync(new QueryRequest
			{
				PageRequest = page,
				Filters = new[]
				{
					new QueryInstance
					{
						Name = "memorySize",
						Comp = equalityType,
						Value = 555
					}
				}
			}, default);

			Assert.AreEqual(totalCount, result.TotalCount);

			VerifyMocks();
		}

		[Test]
		public async Task GetPagedAsync_AppliesEquality()
		{
			var page = new PageRequest
			{
				Count = 10, Next = 10, TotalCount = 0
			};

			const int totalCount = 532;
			const string filterValue = "Crap";

			_repoMock.Setup(r => r.GetPagedPreviewsAsync(
					It.Is<int>(i => i == page.Next),
					It.Is<int>(i => i == page.Count),
					false,
					(Expression<Func<ResultProjection, object>>) null,
					It.Is<IEnumerable<Expression<Func<ResultProjection, bool>>>>(enumerable =>
						enumerable.Any(x => x.Body.ToString().Contains("=="))),
					true,
					default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"))
				}))
				.Verifiable();

			var service = GetService();

			var result = await service.GetPagedAsync(new QueryRequest
			{
				PageRequest = page,
				Filters = new[]
				{
					new QueryInstance
					{
						Name = "name",
						Comp = "eq",
						Value = filterValue
					}
				}
			}, default);

			Assert.AreEqual(totalCount, result.TotalCount);

			VerifyMocks();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("  \t \n")]
		public void GetSingleAsync_InvalidId_ThrowsArgumentException(string id)
		{
			var service = GetService();

			Assert.ThrowsAsync<ArgumentException>(async () => await service.GetSingleAsync(id, default));

			VerifyMocks();
		}

		[Test]
		public async Task DeleteAllAsync_Success()
		{
			_repoMock.Setup(r => r.DeleteAllAsync(default))
				.Returns(Task.CompletedTask)
				.Verifiable();

			var service = GetService();

			await service.ClearResultsAsync(default);

			VerifyMocks();
		}

		[Test]
		public void GetSingleAsync_NonExistent_ThrowsNotFoundException()
		{
			var id = Guid.NewGuid().ToString("N");

			_repoMock.Setup(r =>
					r.GetProjectionAsync(It.Is<string>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => null)
				.Verifiable();

			var service = GetService();

			Assert.ThrowsAsync<NotFoundException>(async () => await service.GetSingleAsync(id, default));

			VerifyMocks();
		}
	}
}