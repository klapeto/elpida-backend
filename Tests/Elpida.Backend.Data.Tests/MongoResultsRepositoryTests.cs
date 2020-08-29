using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Tests.Dummies;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Data.Tests
{
	public class MongoResultsRepositoryTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			BsonClassMap.RegisterClassMap<ResultModel>();
		}

		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new MongoResultsRepository(null));
		}

		[Test]
		public void GetSingleAsync_NullId_ThrowsArgumentException()
		{
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			var repo = new MongoResultsRepository(mock.Object);
			Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetSingleAsync(null, default));
			
			mock.VerifyAll();
			mock.VerifyNoOtherCalls();
		}

		[Test]
		public void GetSingleAsync_EmptyId_ThrowsArgumentException()
		{
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			var repo = new MongoResultsRepository(mock.Object);
			Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetSingleAsync(string.Empty, default));
			
			mock.VerifyAll();
			mock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetSingleAsync_Success()
		{
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);

			mock.Setup(r => r.FindAsync(It.IsAny<FilterDefinition<ResultModel>>(),
					It.IsAny<FindOptions<ResultModel,ResultModel>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<ResultModel>(new List<ResultModel>()))
				.Verifiable();
			var repo = new MongoResultsRepository(mock.Object);
			
			await repo.GetSingleAsync("Haha", default);
			
			mock.VerifyAll();
			mock.VerifyNoOtherCalls();
		}

		[Test]
		public void GetSingleAsync_NullModel_ThrowsArgumentNullException()
		{
			var collection = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			var repo = new MongoResultsRepository(collection.Object);
			Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.CreateAsync(null, default));
			
			collection.VerifyAll();
			collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task CreateAsync_Success()
		{
			var collection = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			collection.Setup(r => r.InsertOneAsync(It.IsAny<ResultModel>(), It.IsAny<InsertOneOptions>(),It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask)
				.Verifiable();
			
			var repo = new MongoResultsRepository(collection.Object);
			await repo.CreateAsync(new ResultModel(), default);
			
			collection.VerifyAll();
			collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetCountAsync_Success()
		{
			var collection = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			collection.Setup(r => r.CountDocumentsAsync(It.IsAny<FilterDefinition<ResultModel>>(), It.IsAny<CountOptions>(),It.IsAny<CancellationToken>()))
				.ReturnsAsync(5)
				.Verifiable();
			
			var repo = new MongoResultsRepository(collection.Object);
			await repo.GetTotalCountAsync(default);
			
			collection.VerifyAll();
			collection.VerifyNoOtherCalls();
		}


		[Test]
		public void GetAsync_InvalidFrom_ThrowsArgumentException()
		{
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			var repo = new MongoResultsRepository(mock.Object);
			Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetAsync(-5, 10, false, default));
			
			mock.VerifyNoOtherCalls();
		}

		[Test]
		public void GetAsync_InvalidCount_ThrowsArgumentException()
		{
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			var repo = new MongoResultsRepository(mock.Object);
			Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetAsync(0, -5, false, default));
			
			mock.VerifyNoOtherCalls();
		}

		[Test]
		public async Task DeleteAllAsync_Success()
		{			
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);

			mock.Setup(r => r.DeleteManyAsync(It.IsAny<FilterDefinition<ResultModel>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DeleteResult.Acknowledged(5))
				.Verifiable();
			
			var repo = new MongoResultsRepository(mock.Object);
			await repo.DeleteAllAsync(default);

			mock.VerifyAll();
			mock.VerifyNoOtherCalls();
		}

		private static Mock<IMongoCollection<ResultModel>> GetDefaultCollectionMock()
		{
			var mock = new Mock<IMongoCollection<ResultModel>>(MockBehavior.Strict);
			
			mock.SetupGet(r => r.CollectionNamespace)
				.Returns(CollectionNamespace.FromFullName("x.x"));
			mock.SetupGet(r => r.Settings)
				.Returns(new MongoCollectionSettings());
			mock.SetupGet(r => r.DocumentSerializer)
				.Returns(new BsonClassMapSerializer<ResultModel>(BsonClassMap.LookupClassMap(typeof(ResultModel))));

			return mock;
		}
		
		[Test]
		public async Task GetAsync_Success()
		{
			var mock = GetDefaultCollectionMock();

			mock.Setup(r => r.AggregateAsync(It.IsAny<PipelineDefinition<ResultModel, ResultPreviewModel>>(),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<ResultPreviewModel>(new List<ResultPreviewModel>()))
				.Verifiable();

			var repo = new MongoResultsRepository(mock.Object);
			await repo.GetAsync(0, 10, false, default);
			
			mock.VerifyAll();
			mock.VerifyNoOtherCalls();
		}
		
		[Test]
		public async Task GetAsync_Desc_Success()
		{
			var mock = GetDefaultCollectionMock();

			mock.Setup(r => r.AggregateAsync(It.IsAny<PipelineDefinition<ResultModel, ResultPreviewModel>>(),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<ResultPreviewModel>(new List<ResultPreviewModel>()))
				.Verifiable();
			
			var repo = new MongoResultsRepository(mock.Object);
			await repo.GetAsync(0, 10, true, default);
			
			mock.VerifyAll();
			mock.VerifyNoOtherCalls();
		}
	}
}