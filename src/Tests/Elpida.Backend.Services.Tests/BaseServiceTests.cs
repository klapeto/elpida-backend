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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Utilities;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	public class BaseServiceTests
	{
		[Test]
		public async Task GetSingleAsync_ExistingId_ReturnsObject()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			const long id = 5;

			repo.Setup(r => r.GetSingleAsync(id, default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = id,
						Data = "lol",
					}
				);

			var service = new DummyService(repo.Object, lockFactory.Object);

			var obj = await service.GetSingleAsync(id);

			Assert.NotNull(obj);

			repo.Verify(r => r.GetSingleAsync(id, default), Times.Once);
		}

		[Test]
		public void GetSingleAsync_NonExistingId_ReturnsObject()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			const long id = 5;

			repo.Setup(r => r.GetSingleAsync(id, default))
				.ReturnsAsync((DummyModel)null);

			var service = new DummyService(repo.Object, lockFactory.Object);

			Assert.ThrowsAsync<NotFoundException>(() => service.GetSingleAsync(id));

			repo.Verify(r => r.GetSingleAsync(id, default), Times.Once);
		}

		[Test]
		public async Task GetPagedAsync_ValidQuery_ReturnsObjects()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			var service = new DummyService(repo.Object, lockFactory.Object);
			var builder = new QueryExpressionBuilder(service.GetImplementedFilters());

			var query = CreateQuery();

			const int totalCount = 20;

			var returnItems = CreateReturnModels();

			repo.Setup(
					r => r.GetMultiplePagedAsync(
						query.PageRequest.Next,
						query.PageRequest.Count,
						query.Descending,
						true,
						It.Is<Expression<Func<DummyModel, object>>?>(
							x => x.ToString() == builder.GetOrderBy<DummyModel>(query).ToString()
						),
						It.Is<IEnumerable<Expression<Func<DummyModel, bool>>>>(
							x => x.SequenceEqual(
								builder.Build<DummyModel>(query.Filters),
								new ExpressionEqualityComparer<DummyModel>()
							)
						),
						default
					)
				)
				.ReturnsAsync(new PagedQueryResult<DummyModel>(totalCount, returnItems));

			var obj = await service.GetPagedAsync(query);

			Assert.NotNull(obj);

			Assert.AreEqual(returnItems.Count, obj.Count);
			Assert.AreEqual(totalCount, obj.TotalCount);

			for (var i = 0; i < returnItems.Count; i++)
			{
				Assert.AreEqual(returnItems[i].Id, obj.List[i].Id);
				Assert.AreEqual(returnItems[i].Data, obj.List[i].Data);
			}

			repo.Verify(
				r => r.GetMultiplePagedAsync(
					query.PageRequest.Next,
					query.PageRequest.Count,
					query.Descending,
					true,
					It.Is<Expression<Func<DummyModel, object>>?>(
						x => x.ToString() == builder.GetOrderBy<DummyModel>(query).ToString()
					),
					It.Is<IEnumerable<Expression<Func<DummyModel, bool>>>>(
						x => x.SequenceEqual(
							builder.Build<DummyModel>(query.Filters),
							new ExpressionEqualityComparer<DummyModel>()
						)
					),
					default
				),
				Times.Once
			);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task GetPagedAsync_Descending_CallsRepoWithCorrectValue(bool descending)
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			var service = new DummyService(repo.Object, lockFactory.Object);

			var query = CreateQuery();

			query.Descending = descending;
			const int totalCount = 20;

			var returnItems = CreateReturnModels();

			repo.Setup(
					r => r.GetMultiplePagedAsync(
						It.IsAny<int>(),
						It.IsAny<int>(),
						descending,
						true,
						It.IsAny<Expression<Func<DummyModel, object>>?>(),
						It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
						default
					)
				)
				.ReturnsAsync(new PagedQueryResult<DummyModel>(totalCount, returnItems));

			_ = await service.GetPagedAsync(query);

			repo.Verify(
				r => r.GetMultiplePagedAsync(
					It.IsAny<int>(),
					It.IsAny<int>(),
					descending,
					true,
					It.IsAny<Expression<Func<DummyModel, object>>?>(),
					It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
					default
				),
				Times.Once
			);
		}

		[Test]
		[TestCase(0, true, 12)]
		[TestCase(5, false, 0)]
		public async Task GetPagedAsync_TotalCount_CalculatedWhenNeeded(int totalCount, bool valuePassed, int expected)
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			var service = new DummyService(repo.Object, lockFactory.Object);

			var query = CreateQuery();

			query.PageRequest.TotalCount = totalCount;

			var returnItems = CreateReturnModels();

			repo.Setup(
					r => r.GetMultiplePagedAsync(
						It.IsAny<int>(),
						It.IsAny<int>(),
						It.IsAny<bool>(),
						valuePassed,
						It.IsAny<Expression<Func<DummyModel, object>>?>(),
						It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
						default
					)
				)
				.ReturnsAsync(new PagedQueryResult<DummyModel>(expected, returnItems));

			var obj = await service.GetPagedAsync(query);

			Assert.AreEqual(expected, obj.TotalCount);

			repo.Verify(
				r => r.GetMultiplePagedAsync(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<bool>(),
					valuePassed,
					It.IsAny<Expression<Func<DummyModel, object>>?>(),
					It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
					default
				),
				Times.Once
			);
		}

		[Test]
		public async Task GetOrAddAsync_NoBypassChecks_Success()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			var service = new DummyService(repo.Object, lockFactory.Object);

			var dto = new DummyDto
			{
				Data = "hahah",
				Id = 5,
			};

			const int actualId = 8;

			repo.Setup(r => r.CreateAsync(It.Is<DummyModel>(m => m.Id == 0 && m.Data == dto.Data), default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = actualId,
						Data = dto.Data,
					}
				);

			repo.Setup(r => r.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var obj = await service.GetOrAddAsync(dto);

			Assert.NotNull(obj);
			Assert.AreEqual(actualId, obj.Id);
			Assert.AreEqual(dto.Data, obj.Data);

			repo.Verify(
				r => r.CreateAsync(It.Is<DummyModel>(m => m.Id == 0 && m.Data == dto.Data), default),
				Times.Once
			);

			repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
		}

		[Test]
		public async Task GetOrAddAsync_BypassCheck_NoExisting_DoesAdd()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);
			var lockMock = new Mock<IDisposable>(MockBehavior.Strict);

			Expression<Func<DummyModel, bool>> bypassCheck = m => true;

			var service = new DummyService(repo.Object, lockFactory.Object, bypassCheck);

			var dto = new DummyDto
			{
				Data = "hahah",
				Id = 5,
			};

			const int actualId = 8;

			repo.Setup(r => r.CreateAsync(It.Is<DummyModel>(m => m.Id == 0 && m.Data == dto.Data), default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = actualId,
						Data = dto.Data,
					}
				);

			lockMock.Setup(l => l.Dispose());

			lockFactory.Setup(l => l.AcquireAsync(nameof(DummyService), default))
				.ReturnsAsync(lockMock.Object);

			repo.Setup(r => r.GetSingleAsync(bypassCheck, default))
				.ReturnsAsync((DummyModel)null!);

			repo.Setup(r => r.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var obj = await service.GetOrAddAsync(dto);

			Assert.NotNull(obj);
			Assert.AreEqual(actualId, obj.Id);
			Assert.AreEqual(dto.Data, obj.Data);

			repo.Verify(r => r.GetSingleAsync(bypassCheck, default), Times.Exactly(2));
			repo.Verify(
				r => r.CreateAsync(It.Is<DummyModel>(m => m.Id == 0 && m.Data == dto.Data), default),
				Times.Once
			);

			repo.Verify(r => r.SaveChangesAsync(default), Times.Once);

			lockFactory.Verify(l => l.AcquireAsync(nameof(DummyService), default), Times.Once);
			lockMock.Verify(l => l.Dispose(), Times.Once);
		}

		[Test]
		public async Task GetOrAddAsync_BypassCheck_ExistingFirst_DoesNotAdd()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			Expression<Func<DummyModel, bool>> bypassCheck = m => true;

			var service = new DummyService(repo.Object, lockFactory.Object, bypassCheck);

			var dto = new DummyDto
			{
				Data = "hahah",
				Id = 5,
			};

			const int actualId = 8;

			repo.Setup(r => r.GetSingleAsync(bypassCheck, default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = actualId,
						Data = dto.Data,
					}
				);

			var obj = await service.GetOrAddAsync(dto);

			Assert.NotNull(obj);
			Assert.AreEqual(actualId, obj.Id);
			Assert.AreEqual(dto.Data, obj.Data);

			repo.Verify(r => r.GetSingleAsync(bypassCheck, default), Times.Once);
		}

		[Test]
		public async Task GetOrAddAsync_BypassCheck_DoubleLockCheck_DoesNotAdd()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);
			var lockMock = new Mock<IDisposable>(MockBehavior.Strict);

			var bypassCalls = 0;
			Expression<Func<DummyModel, bool>> bypassCheck = m => true;

			var service = new DummyService(repo.Object, lockFactory.Object, bypassCheck);

			var dto = new DummyDto
			{
				Data = "hahah",
				Id = 5,
			};

			const int actualId = 8;

			lockMock.Setup(l => l.Dispose());

			lockFactory.Setup(l => l.AcquireAsync(nameof(DummyService), default))
				.ReturnsAsync(lockMock.Object);

			repo.Setup(r => r.GetSingleAsync(bypassCheck, default))
				.ReturnsAsync(
					() => bypassCalls++ > 0
						? new DummyModel
						{
							Id = actualId,
							Data = dto.Data,
						}
						: null
				);

			var obj = await service.GetOrAddAsync(dto);

			Assert.NotNull(obj);
			Assert.AreEqual(actualId, obj.Id);
			Assert.AreEqual(dto.Data, obj.Data);

			repo.Verify(r => r.GetSingleAsync(bypassCheck, default), Times.Exactly(2));

			lockFactory.Verify(l => l.AcquireAsync(nameof(DummyService), default), Times.Once);
			lockMock.Verify(l => l.Dispose(), Times.Once);
		}

		private static List<DummyModel> CreateReturnModels()
		{
			return new ()
			{
				new DummyModel
				{
					Id = 2,
					Data = "lol",
				},
				new DummyModel
				{
					Id = 6,
					Data = "lol",
				},
			};
		}

		private static QueryRequest CreateQuery()
		{
			return new ()
			{
				Descending = false,
				PageRequest = new PageRequest
				{
					Next = 12,
					Count = 2,
					TotalCount = 0,
				},
				OrderBy = "data",
				Filters = new[]
				{
					new QueryInstance
					{
						Comp = "eq",
						Name = "data",
						Value = "lol",
					},
				},
			};
		}

		[Test]
		public void GetFilters_WrongModel_ThrowsArgumentExceptionException()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			var service = new DummyService(repo.Object, lockFactory.Object);

			Assert.Throws<ArgumentException>(() => service.ConstructCustomFilters<DummyModel, string>(m => m.Data).ToArray());
		}

		[Test]
		public void GetFilters_Success()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var lockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

			var service = new DummyService(repo.Object, lockFactory.Object);

			var filters = service.ConstructCustomFilters<DummyModelParent, DummyModel>(m => m.Child)
				.ToArray();

			Assert.AreEqual(filters.Length, 2);

			Assert.AreEqual(filters.First().Expression.ToString(), "m.Child.Id");
			Assert.AreEqual(filters.First().Name, "id");
			
			Assert.AreEqual(filters.Last().Expression.ToString(), "m.Child.Data");
			Assert.AreEqual(filters.Last().Name, "data");
		}
	}
}