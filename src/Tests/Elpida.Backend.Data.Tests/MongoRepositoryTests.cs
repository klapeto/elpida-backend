/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Tests.Dummies;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Data.Tests
{
	public class A : IEntity
	{
		public string Id { get; set; }

		public int Order { get; set; }
	}

	public class MongoRepositoryTests
	{
		private Mock<IMongoCollection<A>> _collection;

		private MongoRepository<A> GetRepo()
		{
			return new MongoRepository<A>(_collection.Object);
		}

		private void PreConfigureCollection()
		{
			_collection.SetupGet(r => r.CollectionNamespace)
				.Returns(CollectionNamespace.FromFullName("x.Collection"));
			_collection.SetupGet(r => r.Settings)
				.Returns(new MongoCollectionSettings());
			_collection.SetupGet(r => r.DocumentSerializer)
				.Returns(new BsonClassMapSerializer<A>(BsonClassMap.LookupClassMap(typeof(A))));
		}

		private static bool AreEqual<T,TR>(PipelineDefinition<T, TR> a, PipelineDefinition<T, TR> b)
		{
			return a.ToString() == b.ToString();
		}

		private static BsonDocumentSortDefinition<A> GetSortObject<T>(Expression<Func<A,T>> field, bool desc)
		{
			dynamic body = field.Body;

			var member = (MemberInfo) body.Member;
			return new BsonDocumentSortDefinition<A>(new BsonDocument(new Dictionary<string, object>
				{[(member.Name.ToLower() == "id" ? "_id" : member.Name)] = (desc ? -1 : 1)}));
		}

		[OneTimeSetUp]
		public void Setup()
		{
			BsonClassMap.RegisterClassMap<A>();
		}

		[SetUp]
		public void CreateMocks()
		{
			_collection = new Mock<IMongoCollection<A>>(MockBehavior.Strict);
		}

		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new MongoRepository<A>(null));
		}

		[Test]
		public void Constructor_ValidArgument_Success()
		{
			_ = new MongoRepository<A>(_collection.Object);
		}

		[Test]
		public void CreateAsync_NullArgument_ThrowsArgumentNullException()
		{
			var repo = GetRepo();
			Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.CreateAsync(null));
		}

		[Test]
		public async Task CreateAsync_ValidArgument_ReturnsId()
		{
			var model = new A
			{
				Id = Guid.NewGuid().ToString("N")
			};

			_collection.Setup(r =>
					r.InsertOneAsync(It.IsAny<A>(), It.IsAny<InsertOneOptions>(),
						It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask)
				.Verifiable();

			var repo = GetRepo();

			var id = await repo.CreateAsync(model);

			Assert.AreEqual(model.Id, id);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetTotalCount_ReturnsValid()
		{
			const int expectedCount = 5;

			_collection.Setup(r => r.CountDocumentsAsync(It.IsAny<FilterDefinition<A>>(),
					It.IsAny<CountOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(expectedCount)
				.Verifiable();

			var repo = GetRepo();

			var count = await repo.GetTotalCountAsync();

			Assert.AreEqual(expectedCount, count);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("\n")]
		[TestCase("\t \n ")]
		public void GetSingleAsync_EmptyId_ThrowsArgumentException(string id)
		{
			var repo = GetRepo();

			Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetSingleAsync(id));

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetSingleAsync_NoResult_ReturnsNull()
		{
			_collection.Setup(r => r.FindAsync(It.IsAny<FilterDefinition<A>>(),
					It.IsAny<FindOptions<A, A>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<A>(new List<A>()))
				.Verifiable();

			var repo = GetRepo();

			var results = await repo.GetSingleAsync("Haha");

			Assert.IsNull(results);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}


		[Test]
		public async Task GetSingleAsync_ValidResult_ReturnsFirstResult()
		{
			var models = new List<A>
			{
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
				},
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
				}
			};

			_collection.Setup(r => r.FindAsync(It.IsAny<FilterDefinition<A>>(),
					It.IsAny<FindOptions<A, A>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<A>(models))
				.Verifiable();

			var repo = GetRepo();

			var resultModel = await repo.GetSingleAsync("haha");

			Assert.NotNull(resultModel);
			Assert.AreEqual(models.First().Id, resultModel.Id);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task DeleteAllAsync_Success()
		{
			_collection.Setup(r =>
					r.DeleteManyAsync(It.IsAny<FilterDefinition<A>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DeleteResult.Acknowledged(5))
				.Verifiable();

			var repo = GetRepo();
			await repo.DeleteAllAsync();

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetAsync_NoFilter_Success()
		{
			PreConfigureCollection();

			var models = new List<A>
			{
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
				},
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
				}
			};

			var matchStr = new EmptyPipelineDefinition<A>().ToString();

			_collection.Setup(r => r.AggregateAsync(It.Is<PipelineDefinition<A, A>>(x => x.ToString() == matchStr),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<A>(models))
				.Verifiable();

			var repo = GetRepo();

			var results = await repo.GetAsync(null);

			Assert.AreEqual(models.Count, results.Count);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetAsync_WithFilter_Success()
		{
			PreConfigureCollection();

			var models = new List<A>
			{
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
				},
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
				}
			};

			const string matchStr = "xsf";

			var filters = new List<Expression<Func<A, bool>>> {a => a.Id == matchStr};

			var matchDef = new EmptyPipelineDefinition<A>()
				.Match(a => a.Id == matchStr);

			_collection.Setup(r => r.AggregateAsync(It.Is<PipelineDefinition<A, A>>(x => AreEqual(x, matchDef)),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<A>(models))
				.Verifiable();

			var repo = GetRepo();

			var results = await repo.GetAsync(filters);

			Assert.AreEqual(models.Count, results.Count);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		[TestCase(-5, 10)]
		[TestCase(0, -8)]
		[TestCase(-1, -8)]
		public void GetPagedAsync_InvalidRange_ThrowsArgumentException(int from, int count)
		{
			var repo = GetRepo();

			Assert.ThrowsAsync<ArgumentException>(async () =>
				await repo.GetPagedAsync<object>(from, count, false, null, null, false));

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}

		[Test]
		public async Task GetPagedAsync_Success()
		{
			PreConfigureCollection();

			const int expectedCount = 50;

			var models = new List<A>
			{
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
					Order = 1
				},
				new A
				{
					Id = Guid.NewGuid().ToString("N"),
					Order = 2
				}
			};

			const int count = 26;
			const int from = 12;
			const bool desc = true;
			const string matchStr = "xsf";
			Expression<Func<A, int>> orderBy = a => a.Order;

			var filters = new List<Expression<Func<A, bool>>> {a => a.Id == matchStr};

			var matchDef = new EmptyPipelineDefinition<A>()
				.Match(a => a.Id == matchStr)
				.Sort(GetSortObject(orderBy, desc))
				.Skip(from)
				.Limit(count);

			_collection.Setup(r => r.AggregateAsync(It.Is<PipelineDefinition<A, A>>(x => AreEqual(x, matchDef)),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<A>(models))
				.Verifiable();

			_collection.Setup(r => r.AggregateAsync(It.IsAny<PipelineDefinition<A, int>>(),
					It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new DummyAsyncCursor<int>(new[] {expectedCount}))
				.Verifiable();

			var repo = GetRepo();

			var results = await repo.GetPagedAsync(from, count, desc, orderBy,
				filters, true);

			Assert.AreEqual(models.Count, results.Items.Count);
			Assert.AreEqual(expectedCount, results.TotalCount);

			_collection.VerifyAll();
			_collection.VerifyNoOtherCalls();
		}
	}
}