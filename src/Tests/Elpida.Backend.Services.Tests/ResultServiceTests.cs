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
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Extensions.Result;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultServiceTests
	{
		private Mock<IResultsRepository> _repoMock;
		private Mock<ICpuRepository> _cpuMock;
		private Mock<ITopologyRepository> _topologyMock;
		private Mock<IBenchmarkRepository> _benchmarkMock;


		[SetUp]
		public void CreateMocks()
		{
			_repoMock = new Mock<IResultsRepository>(MockBehavior.Strict);
			_cpuMock = new Mock<ICpuRepository>(MockBehavior.Strict);
			_topologyMock = new Mock<ITopologyRepository>(MockBehavior.Strict);
			_benchmarkMock = new Mock<IBenchmarkRepository>(MockBehavior.Strict);
		}

		private BenchmarkResultService GetService()
		{
			return new BenchmarkResultService(_repoMock.Object, _cpuMock.Object, _topologyMock.Object, _benchmarkMock.Object);
		}

		private void VerifyMocks()
		{
			_repoMock.VerifyAll();
			_repoMock.VerifyNoOtherCalls();
			_cpuMock.VerifyAll();
			_cpuMock.VerifyNoOtherCalls();
			_topologyMock.VerifyAll();
			_topologyMock.VerifyNoOtherCalls();
			_benchmarkMock.VerifyAll();
			_benchmarkMock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task CreateAsync_CpuNotExist_CreatesCpu()
		{
			var resultId = 410;

			var dummyDto = Generators.CreateNewResultDto();
			var topology = Generators.CreateNewTopology();
			var cpu = topology.Cpu;

			_repoMock.Setup(r =>
					r.CreateAsync(It.Is<BenchmarkResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new BenchmarkResultModel
				{
					Id = resultId
				})
				.Verifiable();

			_benchmarkMock.Setup(r => r.GetSingleAsync(model => model.Uuid == dummyDto.Result.Uuid, default))
				.ReturnsAsync(new BenchmarkModel
				{
					Id = 41,
					Name = "LOL",
					Uuid = dummyDto.Result.Uuid,
					Tasks = dummyDto.Result.TaskResults.Select(c => new TaskModel
					{
						Id = c.Id,
						Uuid = c.Uuid,
						Description = c.Description,
						Name = c.Name,
						InputDescription = c.Input?.Description,
						InputName = c.Input?.Name,
						InputUnit = c.Input?.Unit,
						InputProperties = "[]",
						OutputDescription = c.Output?.Description,
						OutputName = c.Output?.Name,
						OutputUnit = c.Output?.Unit,
						OutputProperties = "[]",
						ResultAggregation = c.Result.Aggregation,
						ResultDescription = c.Result.Description,
						ResultName = c.Result.Description,
						ResultType = c.Result.Type,
						ResultUnit = c.Result.Unit
					}).ToList()
				});

			_cpuMock.Setup(i => i.GetSingleAsync(It.IsAny<Expression<Func<CpuModel, bool>>>(), default))
				.ReturnsAsync((CpuModel) null)
				.Verifiable();

			_cpuMock.Setup(i => i.CreateAsync(It.IsAny<CpuModel>(), default))
				.ReturnsAsync(cpu)
				.Verifiable();

			_topologyMock.Setup(i =>
					i.GetSingleAsync(It.IsAny<Expression<Func<TopologyModel, bool>>>(), default))
				.ReturnsAsync(topology)
				.Verifiable();
			
			_repoMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);
			_cpuMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);
			_topologyMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var service = GetService();

			var result = await service.CreateAsync(dummyDto, default);

			Assert.AreEqual(resultId, result);

			VerifyMocks();
		}

		[Test]
		public async Task CreateAsync_TopologyNotExist_CreatesTopology()
		{
			var resultId = 410;

			var dummyDto = Generators.CreateNewResultDto();
			var topology = Generators.CreateNewTopology();
			var cpu = topology.Cpu;
			
			_repoMock.Setup(r =>
					r.CreateAsync(It.Is<BenchmarkResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new BenchmarkResultModel
				{
					Id = resultId
				})
				.Verifiable();

			
			_benchmarkMock.Setup(r => r.GetSingleAsync(model => model.Uuid == dummyDto.Result.Uuid, default))
				.ReturnsAsync(new BenchmarkModel
				{
					Id = 41,
					Name = "LOL",
					Uuid = dummyDto.Result.Uuid,
					Tasks = dummyDto.Result.TaskResults.Select(c => new TaskModel
					{
						Id = c.Id,
						Uuid = c.Uuid,
						Description = c.Description,
						Name = c.Name,
						InputDescription = c.Input?.Description,
						InputName = c.Input?.Name,
						InputUnit = c.Input?.Unit,
						InputProperties = "[]",
						OutputDescription = c.Output?.Description,
						OutputName = c.Output?.Name,
						OutputUnit = c.Output?.Unit,
						OutputProperties = "[]",
						ResultAggregation = c.Result.Aggregation,
						ResultDescription = c.Result.Description,
						ResultName = c.Result.Description,
						ResultType = c.Result.Type,
						ResultUnit = c.Result.Unit
					}).ToList()
				});

			_cpuMock.Setup(i => i.GetSingleAsync(It.IsAny<Expression<Func<CpuModel, bool>>>(), default))
				.ReturnsAsync(cpu)
				.Verifiable();

			_topologyMock.Setup(i =>
					i.GetSingleAsync(It.IsAny<Expression<Func<TopologyModel, bool>>>(), default))
				.ReturnsAsync((TopologyModel)null)
				.Verifiable();
			
			_topologyMock.Setup(i => i.CreateAsync(It.IsAny<TopologyModel>(), default))
				.ReturnsAsync(topology)
				.Verifiable();
			
			_repoMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);
			_cpuMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);
			_topologyMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var service = GetService();

			var result = await service.CreateAsync(dummyDto, default);

			Assert.AreEqual(resultId, result);

			VerifyMocks();
		}

		[Test]
		public async Task CreateAsync_Success()
		{
			var resultId = 410;
	
			var dummyDto = Generators.CreateNewResultDto();
			var topology = Generators.CreateNewTopology();
			var cpu = topology.Cpu;
			
			_repoMock.Setup(r =>
					r.CreateAsync(It.Is<BenchmarkResultModel>(m => m.TimeStamp != default), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new BenchmarkResultModel
				{
					Id = resultId
				})
				.Verifiable();
			
			_benchmarkMock.Setup(r => r.GetSingleAsync(model => model.Uuid == dummyDto.Result.Uuid, default))
				.ReturnsAsync(new BenchmarkModel
				{
					Id = 41,
					Name = "LOL",
					Uuid = dummyDto.Result.Uuid,
					Tasks = dummyDto.Result.TaskResults.Select(c => new TaskModel
					{
						Id = c.Id,
						Uuid = c.Uuid,
						Description = c.Description,
						Name = c.Name,
						InputDescription = c.Input?.Description,
						InputName = c.Input?.Name,
						InputUnit = c.Input?.Unit,
						InputProperties = "[]",
						OutputDescription = c.Output?.Description,
						OutputName = c.Output?.Name,
						OutputUnit = c.Output?.Unit,
						OutputProperties = "[]",
						ResultAggregation = c.Result.Aggregation,
						ResultDescription = c.Result.Description,
						ResultName = c.Result.Description,
						ResultType = c.Result.Type,
						ResultUnit = c.Result.Unit
					}).ToList()
				});

			_cpuMock.Setup(i => i.GetSingleAsync(It.IsAny<Expression<Func<CpuModel, bool>>>(), default))
				.ReturnsAsync(cpu)
				.Verifiable();

			_topologyMock.Setup(i => i.GetSingleAsync(It.IsAny<Expression<Func<TopologyModel, bool>>>(), default))
				.ReturnsAsync(topology)
				.Verifiable();
			
			_repoMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);
			_cpuMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);
			_topologyMock.Setup(c => c.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var service = GetService();

			var result = await service.CreateAsync(dummyDto, default);

			Assert.AreEqual(resultId, result);

			VerifyMocks();
		}

		[Test]
		public async Task GetSingleAsync_Success()
		{
			var id = 1654;

			_repoMock.Setup(r =>
					r.GetSingleAsync(It.Is<long>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => Generators.CreateNewResultModel(id))
				.Verifiable();

			var service = GetService();

			var result = await service.GetSingleAsync(id, default);

			Assert.AreEqual(id, result.Id);

			VerifyMocks();
		}

		[Test]
		public void GetSingleAsync_NonExist_ThrowsNotFoundException()
		{
			var id = 464;

			_repoMock.Setup(r =>
					r.GetSingleAsync(It.Is<long>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => null)
				.Verifiable();

			var service = GetService();

			Assert.ThrowsAsync<NotFoundException>(async () => await service.GetSingleAsync(id, default));

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
						It.Is<IEnumerable<Expression<Func<BenchmarkResultModel, bool>>>>(e => !e.Any()),
						false,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(0, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(54654)
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
						It.Is<IEnumerable<Expression<Func<BenchmarkResultModel, bool>>>>(e => !e.Any()),
						true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(546554)
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
						It.Is<IEnumerable<Expression<Func<BenchmarkResultModel, bool>>>>(enumerable =>
							enumerable.Any(x => x.Body.ToString().Contains(filterValue))),
						true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(54684)
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
						It.Is<IEnumerable<Expression<Func<BenchmarkResultModel, bool>>>>(enumerable =>
							enumerable.Any(x => x.Body.ToString().Contains(filterValue))),
						true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(464847)
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
						It.Is<IEnumerable<Expression<Func<BenchmarkResultModel, bool>>>>(enumerable =>
							enumerable.Any(x => x.Body.ToString().Contains(stringToCheck))), true,
						default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(44466)
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
					(Expression<Func<BenchmarkResultModel, object>>) null,
					It.Is<IEnumerable<Expression<Func<BenchmarkResultModel, bool>>>>(enumerable =>
						enumerable.Any(x => x.Body.ToString().Contains("=="))),
					true,
					default))
				.ReturnsAsync(() => new PagedQueryResult<ResultPreviewModel>(totalCount, new List<ResultPreviewModel>
				{
					Generators.CreateResultPreviewModel(46464)
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
		public void GetSingleAsync_NonExistent_ThrowsNotFoundException()
		{
			var id = 65465;

			_repoMock.Setup(r =>
					r.GetSingleAsync(It.Is<long>(i => i == id), It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => null)
				.Verifiable();

			var service = GetService();

			Assert.ThrowsAsync<NotFoundException>(async () => await service.GetSingleAsync(id, default));

			VerifyMocks();
		}
	}
}