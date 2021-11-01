using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;
using Elpida.Web.Frontend.Shared.Exceptions;

namespace Elpida.Web.Frontend.Services
{
	public abstract class FrontEndServiceBase<TDto, TPreview> : IFrontendService<TDto, TPreview>
		where TDto : FoundationDto
		where TPreview : FoundationDto
	{
		protected FrontEndServiceBase(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

		protected HttpClient HttpClient { get; }

		protected abstract string UrlRouteBase { get; }

		public Task<TDto> GetSingleAsync(long id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public async Task<PagedResult<TPreview>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			var response = await HttpClient.PostAsync(
				$"{UrlRouteBase}/Search",
				JsonContent.Create(queryRequest),
				cancellationToken
			);

			if (response is null)
			{
				throw new InvalidResponseException();
			}

			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<PagedResult<TPreview>>(
				       cancellationToken: cancellationToken
			       )
			       ?? throw new InvalidResponseException();
		}

		public Task<TDto> GetOrAddAsync(TDto dto, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<FilterExpression> ConstructCustomFilters<T, TR>(Expression<Func<T, TR>> baseExpression)
		{
			throw new NotSupportedException();
		}

		public abstract QueryModel CreateSimpleQueryModel();

		public abstract QueryModel CreateAdvancedQueryModel();
	}
}