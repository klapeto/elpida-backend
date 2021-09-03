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
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Tests.Helpers;
using Elpida.Backend.Services.Utilities;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	internal class BaseServiceTests
	{
		[Test]
		public async Task GetSingleAsync_ExistingId_ReturnsObject()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			const long id = 5;

			repo.Setup(r => r.GetSingleAsync(id, default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = id,
						Data = "lol",
					}
				);

			var service = new DummyService(repo.Object);

			var obj = await service.GetSingleAsync(id);

			Assert.NotNull(obj);

			repo.Verify(r => r.GetSingleAsync(id, default), Times.Once);
		}

		[Test]
		public void GetSingleAsync_NonExistingId_ThrowsNotFoundException()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			const long id = 5;

			repo.Setup(r => r.GetSingleAsync(id, default))
				.Returns(Task.FromResult((DummyModel?)null));

			var service = new DummyService(repo.Object);

			Assert.ThrowsAsync<NotFoundException>(() => service.GetSingleAsync(id));

			repo.Verify(r => r.GetSingleAsync(id, default), Times.Once);
		}

		[Test]
		public async Task GetPagedPreviewsAsync_ValidQuery_ReturnsObjects()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyService(repo.Object);
			var builder = new QueryExpressionBuilder(service.GetImplementedFilters());

			var query = CreateQuery();

			const int totalCount = 20;

			var returnItems = CreateReturnPreviewDtos();

			repo.Setup(
					r => r.GetPagedProjectionAsync(
						query.PageRequest.Next,
						query.PageRequest.Count,
						It.Is<Expression<Func<DummyModel, DummyPreviewDto>>>(
							x => x.ToString() == service.GetPreviewConstructionExpressionImpl().ToString()
						),
						query.Descending,
						true,
						It.Is<Expression<Func<DummyModel, object>>?>(
							x => x!.ToString() == builder.GetOrderBy<DummyModel>(query)!.ToString()
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
				.ReturnsAsync(new PagedQueryResult<DummyPreviewDto>(totalCount, returnItems));

			var obj = await service.GetPagedPreviewsAsync(query);

			Assert.NotNull(obj);

			Assert.AreEqual(returnItems.Count, obj.Count);
			Assert.AreEqual(totalCount, obj.TotalCount);

			for (var i = 0; i < returnItems.Count; i++)
			{
				Assert.AreEqual(returnItems[i].Id, obj.Items[i].Id);
				Assert.AreEqual(returnItems[i].Data, obj.Items[i].Data);
			}

			repo.Verify(
				r => r.GetPagedProjectionAsync(
					query.PageRequest.Next,
					query.PageRequest.Count,
					It.Is<Expression<Func<DummyModel, DummyPreviewDto>>>(
						x => x.ToString() == service.GetPreviewConstructionExpressionImpl().ToString()
					),
					query.Descending,
					true,
					It.Is<Expression<Func<DummyModel, object>>?>(
						x => x!.ToString() == builder.GetOrderBy<DummyModel>(query)!.ToString()
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
		public async Task GetPagedAsyncBasic_ValidQuery_ReturnsObjects()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyBasicService(repo.Object);

			var query = new QueryRequest(
				new PageRequest(12, 2, 0),
				null,
				null,
				false
			);

			const int totalCount = 20;

			var returnItems = CreateReturnPreviewDtos();

			repo.Setup(
					r => r.GetPagedProjectionAsync(
						It.IsAny<int>(),
						It.IsAny<int>(),
						It.IsAny<Expression<Func<DummyModel, DummyPreviewDto>>>(),
						query.Descending,
						true,
						It.IsAny<Expression<Func<DummyModel, object>>?>(),
						It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
						default
					)
				)
				.ReturnsAsync(new PagedQueryResult<DummyPreviewDto>(totalCount, returnItems));

			var obj = await service.GetPagedPreviewsAsync(query);

			Assert.NotNull(obj);

			Assert.AreEqual(returnItems.Count, obj.Count);
			Assert.AreEqual(totalCount, obj.TotalCount);

			for (var i = 0; i < returnItems.Count; i++)
			{
				Assert.AreEqual(returnItems[i].Id, obj.Items[i].Id);
				Assert.AreEqual(returnItems[i].Data, obj.Items[i].Data);
			}

			repo.Verify(
				r => r.GetPagedProjectionAsync(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<Expression<Func<DummyModel, DummyPreviewDto>>>(),
					query.Descending,
					true,
					It.IsAny<Expression<Func<DummyModel, object>>?>(),
					It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
					default
				),
				Times.Once
			);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task GetPagedPreviewsAsync_Descending_CallsRepoWithCorrectValue(bool descending)
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyService(repo.Object);

			var query = CreateQuery(descending: descending);

			const int totalCount = 20;

			var returnItems = CreateReturnPreviewDtos();

			repo.Setup(
					r => r.GetPagedProjectionAsync(
						It.IsAny<int>(),
						It.IsAny<int>(),
						It.IsAny<Expression<Func<DummyModel, DummyPreviewDto>>>(),
						descending,
						true,
						It.IsAny<Expression<Func<DummyModel, object>>?>(),
						It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
						default
					)
				)
				.ReturnsAsync(new PagedQueryResult<DummyPreviewDto>(totalCount, returnItems));

			_ = await service.GetPagedPreviewsAsync(query);

			repo.Verify(
				r => r.GetPagedProjectionAsync(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<Expression<Func<DummyModel, DummyPreviewDto>>>(),
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

			var service = new DummyService(repo.Object);

			var query = CreateQuery(totalCount);

			var returnItems = CreateReturnPreviewDtos();

			repo.Setup(
					r => r.GetPagedProjectionAsync(
						It.IsAny<int>(),
						It.IsAny<int>(),
						It.IsAny<Expression<Func<DummyModel, DummyPreviewDto>>>(),
						query.Descending,
						valuePassed,
						It.IsAny<Expression<Func<DummyModel, object>>?>(),
						It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
						default
					)
				)
				.ReturnsAsync(new PagedQueryResult<DummyPreviewDto>(expected, returnItems));

			var obj = await service.GetPagedPreviewsAsync(query);

			Assert.AreEqual(expected, obj.TotalCount);

			repo.Verify(
				r => r.GetPagedProjectionAsync(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<Expression<Func<DummyModel, DummyPreviewDto>>>(),
					query.Descending,
					valuePassed,
					It.IsAny<Expression<Func<DummyModel, object>>?>(),
					It.IsAny<IEnumerable<Expression<Func<DummyModel, bool>>>>(),
					default
				),
				Times.Once
			);
		}

		[Test]
		public async Task GetOrAddAsyncBasic_NoBypassChecks_Success()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyBasicService(repo.Object);

			var dto = new DummyDto(5, "hahah");

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
		public async Task GetOrAddAsync_NoBypassChecks_Success()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyService(repo.Object);

			var dto = new DummyDto(5, "hahah");

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
			var transactionMock = new Mock<ITransaction>(MockBehavior.Strict);

			Expression<Func<DummyModel, bool>> bypassCheck = m => true;

			var service = new DummyService(repo.Object, bypassCheck);

			var dto = new DummyDto(5, "hahah");

			const int actualId = 8;

			repo.Setup(r => r.BeginTransactionAsync(default))
				.ReturnsAsync(transactionMock.Object);

			repo.Setup(r => r.CreateAsync(It.Is<DummyModel>(m => m.Id == 0 && m.Data == dto.Data), default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = actualId,
						Data = dto.Data,
					}
				);

			transactionMock.Setup(t => t.Dispose());
			transactionMock.Setup(t => t.CommitAsync(default))
				.Returns(Task.CompletedTask);

			repo.Setup(r => r.GetSingleAsync(bypassCheck, default))
				.ReturnsAsync((DummyModel)null!);

			repo.Setup(r => r.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var obj = await service.GetOrAddAsync(dto);

			Assert.NotNull(obj);
			Assert.AreEqual(actualId, obj.Id);
			Assert.AreEqual(dto.Data, obj.Data);

			repo.Verify(r => r.GetSingleAsync(bypassCheck, default), Times.Once);
			repo.Verify(
				r => r.CreateAsync(It.Is<DummyModel>(m => m.Id == 0 && m.Data == dto.Data), default),
				Times.Once
			);

			repo.Verify(r => r.SaveChangesAsync(default), Times.Once);

			transactionMock.Verify(t => t.Dispose(), Times.Once);
			transactionMock.Verify(t => t.CommitAsync(default), Times.Once);
		}

		[Test]
		public async Task GetOrAddAsync_BypassCheck_ExistingFirst_DoesNotAdd()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);
			var transactionMock = new Mock<ITransaction>(MockBehavior.Strict);

			Expression<Func<DummyModel, bool>> bypassCheck = m => true;

			var service = new DummyService(repo.Object, bypassCheck);

			var dto = new DummyDto(5, "hahah");

			const int actualId = 8;

			repo.Setup(r => r.GetSingleAsync(bypassCheck, default))
				.ReturnsAsync(
					new DummyModel
					{
						Id = actualId,
						Data = dto.Data,
					}
				);

			repo.Setup(r => r.BeginTransactionAsync(default))
				.ReturnsAsync(transactionMock.Object);

			transactionMock.Setup(t => t.Dispose());

			var obj = await service.GetOrAddAsync(dto);

			Assert.NotNull(obj);
			Assert.AreEqual(actualId, obj.Id);
			Assert.AreEqual(dto.Data, obj.Data);

			repo.Verify(r => r.GetSingleAsync(bypassCheck, default), Times.Once);
		}

		[Test]
		public void GetFilters_WrongModel_ThrowsArgumentExceptionException()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyService(repo.Object);

			Assert.Throws<ArgumentException>(
				() => service.ConstructCustomFilters<DummyModel, string>(m => m.Data).ToArray()
			);
		}

		[Test]
		public void GetFilters_Success()
		{
			var repo = new Mock<IDummyRepository>(MockBehavior.Strict);

			var service = new DummyService(repo.Object);

			var filters = service.ConstructCustomFilters<DummyModelParent, DummyModel>(m => m.Child)
				.ToArray();

			Assert.AreEqual(filters.Length, 2);

			Assert.AreEqual(filters.First().Expression.ToString(), "m.Child.Id");
			Assert.AreEqual(filters.First().Name, "id");

			Assert.AreEqual(filters.Last().Expression.ToString(), "m.Child.Data");
			Assert.AreEqual(filters.Last().Name, "data");
		}

		private static List<DummyPreviewDto> CreateReturnPreviewDtos()
		{
			return new ()
			{
				new DummyPreviewDto(2, "lol"),
				new DummyPreviewDto(5, "lolol"),
			};
		}

		private static QueryRequest CreateQuery(long totalCount = 0, bool descending = false)
		{
			var filters = new[] { new FilterInstance("data", "lol", "equal") };
			return new QueryRequest(new PageRequest(12, 2, totalCount), filters, "data", descending);
		}
	}
}