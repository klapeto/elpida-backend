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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Tests.Dummies;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Data.Tests
{
	public class MongoResultsRepositoryTests
	{
		private Mock<IMongoCollection<ResultModel>> _resultMock;
		private Mock<IMongoCollection<CpuModel>> _cpuMock;
		private Mock<IMongoCollection<TopologyModel>> _topologyMock;

		[OneTimeSetUp]
		public void Setup()
		{
			BsonClassMap.RegisterClassMap<ResultModel>();
		}
		
		[SetUp]
		public void CreateMocks()
		{
			_resultMock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			_cpuMock = new Mock<IMongoCollection<CpuModel>>(MockBehavior.Strict);
			_topologyMock = new Mock<IMongoCollection<TopologyModel>>(MockBehavior.Strict);
		}

		private MongoResultsRepository GetRepo()
		{
			return new MongoResultsRepository(_resultMock.Object, _cpuMock.Object, _topologyMock.Object);
		}

		private void VerifyOtherCalls()
		{
			_resultMock.VerifyAll();
			_resultMock.VerifyNoOtherCalls();
			_cpuMock.VerifyAll();
			_cpuMock.VerifyNoOtherCalls();
			_topologyMock.VerifyAll();
			_topologyMock.VerifyNoOtherCalls();
		}

		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new MongoResultsRepository(null,
				Mock.Of<IMongoCollection<CpuModel>>(),
				Mock.Of<IMongoCollection<TopologyModel>>()));
		}

		private void ConfigureDefaultResultCollection(bool withCpu = true, bool withTopology = true)
		{
			_resultMock.SetupGet(r => r.CollectionNamespace)
				.Returns(CollectionNamespace.FromFullName("x.Results"));
			_resultMock.SetupGet(r => r.Settings)
				.Returns(new MongoCollectionSettings());
			_resultMock.SetupGet(r => r.DocumentSerializer)
				.Returns(new BsonClassMapSerializer<ResultModel>(BsonClassMap.LookupClassMap(typeof(ResultModel))));

			if (withCpu)
			{
				_cpuMock.SetupGet(r => r.CollectionNamespace)
					.Returns(CollectionNamespace.FromFullName("x.Cpus"));
				_cpuMock.SetupGet(r => r.DocumentSerializer)
					.Returns(new BsonClassMapSerializer<CpuModel>(BsonClassMap.LookupClassMap(typeof(CpuModel))));
			}

			if (withTopology)
			{
				_topologyMock.SetupGet(r => r.CollectionNamespace)
					.Returns(CollectionNamespace.FromFullName("x.Topologies"));
				_topologyMock.SetupGet(r => r.DocumentSerializer)
					.Returns(new BsonClassMapSerializer<TopologyModel>(BsonClassMap.LookupClassMap(typeof(TopologyModel))));	
			}
		}

		[Test]
		public async Task GetPagedPreviewsAsync_Success()
		{
			ConfigureDefaultResultCollection();

			_resultMock.Setup(r => r.AggregateAsync(It.IsAny<PipelineDefinition<ResultModel, ResultPreviewModel>>(),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<ResultPreviewModel>(new List<ResultPreviewModel>()))
				.Verifiable();
			
			var repo = GetRepo();
			await repo.GetPagedPreviewsAsync<object>(0, 10, false, null, null, false);

			VerifyOtherCalls();
		}
		
		[Test]
		public async Task GetPagedPreviewsAsync_WithFilters_Success()
		{
			ConfigureDefaultResultCollection();

			const int expectedCount = 50;
			
			var models = new List<ResultPreviewModel>
			{
				new ResultPreviewModel
				{
					Id = Guid.NewGuid().ToString("N"),
				},
				new ResultPreviewModel
				{
					Id = Guid.NewGuid().ToString("N"),
				}
			};

			const int count = 26;
			const int from = 12;
			const bool desc = true;
			const string matchStr = "xsf";
			Expression<Func<ResultProjection, DateTime>> orderBy = a => a.TimeStamp;

			var filters = new List<Expression<Func<ResultProjection, bool>>> {a => a.Id == matchStr};

			_resultMock.Setup(r => r.AggregateAsync(
					It.IsAny<PipelineDefinition<ResultModel, ResultPreviewModel>>(),
					It.IsAny<AggregateOptions>(), 
					It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<ResultPreviewModel>(models))
				.Verifiable();
			
			_resultMock.Setup(r => r.AggregateAsync(It.IsAny<PipelineDefinition<ResultModel, int>>(),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<int>(new[] {expectedCount}))
				.Verifiable();

			var repo = GetRepo();

			var results = await repo.GetPagedPreviewsAsync(
				from, 
				count, 
				desc, 
				orderBy,
				filters, 
				true);

			Assert.AreEqual(models.Count, results.Items.Count);
			Assert.AreEqual(expectedCount, results.TotalCount);
			
			VerifyOtherCalls();
		}
		
		[Test]
		[TestCase(-5, 10)]
		[TestCase(0, -8)]
		[TestCase(-1, -8)]
		public void GetPagedPreviewsAsync_InvalidRange_ThrowsArgumentException(int from, int count)
		{
			var repo = GetRepo();

			Assert.ThrowsAsync<ArgumentException>(async () =>
				await repo.GetPagedPreviewsAsync<object>(from, count, false, null, null, false));

			VerifyOtherCalls();
		}
		
		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("\n")]
		[TestCase("\t \n ")]
		public void GetProjectionAsync_EmptyId_ThrowsArgumentException(string id)
		{
			var repo = GetRepo();

			Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetProjectionAsync(id));
			
			VerifyOtherCalls();
		}
		
		[Test]
		public async Task GetGetProjectionAsync_Success()
		{
			ConfigureDefaultResultCollection();

			var models = new List<ResultProjection>
			{
				new ResultProjection
				{
					Id = Guid.NewGuid().ToString("N"),
				}
			};

			_resultMock.Setup(r => r.AggregateAsync(
					It.IsAny<PipelineDefinition<ResultModel, ResultProjection>>(),
					It.IsAny<AggregateOptions>(), 
					It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<ResultProjection>(models))
				.Verifiable();
			
			var repo = GetRepo();

			var result = await repo.GetProjectionAsync(models.First().Id);
			
			Assert.NotNull(result);
			Assert.AreEqual(models.First().Id, result.Id);

			VerifyOtherCalls();
		}
	}
}