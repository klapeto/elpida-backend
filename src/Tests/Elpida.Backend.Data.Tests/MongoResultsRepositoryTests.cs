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
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
		
		private static SortDefinition<T> GetSortObject<T>(Expression<Func<T, object>> field, bool desc)
		{
			var builder = new SortDefinitionBuilder<T>();
			
			return desc ? builder.Descending(field) : builder.Ascending(field);
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
		
		private static bool AreEqual<T,TR>(PipelineDefinition<T, TR> a, PipelineDefinition<T, TR> b)
		{
			return a.ToString() == b.ToString();
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
	}
}