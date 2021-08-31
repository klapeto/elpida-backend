using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Controllers;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Tests;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Controllers
{
	[TestFixture]
	internal abstract class ServiceControllerTests<TDto, TPreview, TService>
		where TDto : FoundationDto
		where TPreview : FoundationDto
		where TService : class, IService<TDto, TPreview>
	{
		[Test]
		public async Task GetSingle_Success()
		{
			var service = new Mock<TService>(MockBehavior.Strict);
			var controller = GetController(service.Object);

			const int id = 5;
			var expectedResult = NewDummyDto();

			service.Setup(s => s.GetSingleAsync(id, default))
				.ReturnsAsync(expectedResult);

			var result = await controller.GetSingle(id, default);

			Assert.AreEqual(expectedResult, result);

			service.Verify(s => s.GetSingleAsync(id, default), Times.Once);
		}

		[Test]
		public void GetSingle_ThrowsException_NoCatch()
		{
			var service = new Mock<TService>(MockBehavior.Strict);
			var controller = GetController(service.Object);

			const int id = 5;
			var expectedException = new NotFoundException("It was not found", id);

			service.Setup(s => s.GetSingleAsync(id, default))
				.Throws(expectedException);

			var actualException = Assert.ThrowsAsync<NotFoundException>(() => controller.GetSingle(id, default));

			Assert.AreEqual(expectedException, actualException);

			service.Verify(s => s.GetSingleAsync(id, default), Times.Once);
		}

		[Test]
		public async Task GetPagedPreviews_Success()
		{
			var service = new Mock<TService>(MockBehavior.Strict);
			var controller = GetController(service.Object);

			var page = DtoGenerators.NewPage();
			var expectedResult = new PagedResult<TPreview>(
				new[] { NewDummyPreviewDto() },
				page
			);

			service.Setup(s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(x => x.PageRequest == page), default))
				.ReturnsAsync(expectedResult);

			var result = await controller.GetPagedPreviews(page, default);

			Assert.AreEqual(expectedResult, result);

			service.Verify(
				s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(x => x.PageRequest == page), default),
				Times.Once
			);
		}

		[Test]
		public void GetPagedPreviews_ThrowsException_NoCatch()
		{
			var service = new Mock<TService>(MockBehavior.Strict);
			var controller = GetController(service.Object);

			var page = DtoGenerators.NewPage();
			var expectedException = new NotFoundException("It was not found", 65);

			service.Setup(s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(x => x.PageRequest == page), default))
				.Throws(expectedException);

			var actualException =
				Assert.ThrowsAsync<NotFoundException>(() => controller.GetPagedPreviews(page, default));

			Assert.AreEqual(expectedException, actualException);

			service.Verify(
				s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(x => x.PageRequest == page), default),
				Times.Once
			);
		}

		[Test]
		public async Task Search_Success()
		{
			var service = new Mock<TService>(MockBehavior.Strict);
			var controller = GetController(service.Object);

			var query = DtoGenerators.NewQueryRequest();
			var expectedResult = new PagedResult<TPreview>(
				new[] { NewDummyPreviewDto() },
				query.PageRequest
			);

			service.Setup(s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(q => QueriesAreEqual(q, query)), default))
				.ReturnsAsync(expectedResult);

			var result = await controller.Search(query, default);

			Assert.AreEqual(expectedResult, result);

			service.Verify(
				s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(q => QueriesAreEqual(q, query)), default),
				Times.Once
			);
		}

		[Test]
		public void Search_ThrowsException_NoCatch()
		{
			var service = new Mock<TService>(MockBehavior.Strict);
			var controller = GetController(service.Object);

			var query = DtoGenerators.NewQueryRequest();
			var expectedException = new NotFoundException("It was not found", 65);

			service.Setup(s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(q => QueriesAreEqual(q, query)), default))
				.Throws(expectedException);

			var actualException = Assert.ThrowsAsync<NotFoundException>(() => controller.Search(query, default));

			Assert.AreEqual(expectedException, actualException);

			service.Verify(
				s => s.GetPagedPreviewsAsync(It.Is<QueryRequest>(q => QueriesAreEqual(q, query)), default),
				Times.Once
			);
		}

		protected abstract ServiceController<TDto, TPreview, TService> GetController(TService service);

		protected abstract TDto NewDummyDto();

		protected abstract TPreview NewDummyPreviewDto();

		private static bool QueriesAreEqual(QueryRequest a, QueryRequest b)
		{
			return a.Descending == b.Descending
			       && a.OrderBy == b.OrderBy
			       && PagesAreEqual(a.PageRequest, b.PageRequest)
			       && FiltersAreEqual(a.Filters, b.Filters);
		}

		private static bool FiltersAreEqual(FilterInstance[]? a, FilterInstance[]? b)
		{
			if (a == null && b == null)
			{
				return true;
			}

			if (a == null && b != null)
			{
				return false;
			}

			if (a != null && b == null)
			{
				return false;
			}

			if (a.Length != b.Length)
			{
				return false;
			}

			for (var i = 0; i < a.Length; i++)
			{
				if (a[i].Comparison != b[i].Comparison
				    || a[i].Name != b[i].Name
				    || a[i].Value != b[i].Value
				)
				{
					return false;
				}
			}

			return true;
		}

		private static bool PagesAreEqual(PageRequest a, PageRequest b)
		{
			return a.Count == b.Count
			       && a.Next == b.Next
			       && a.TotalCount == b.TotalCount;
		}
	}
}