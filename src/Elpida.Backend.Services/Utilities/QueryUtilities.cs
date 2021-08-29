using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend.Services.Utilities
{
	public static class QueryUtilities
	{
		public static async Task<PagedResult<TProjection>> GetPagedProjectionsByPageAsync<TProjection, TModel>(
			IRepository<TModel> repository,
			PageRequest pageRequest,
			bool descending,
			Expression<Func<TModel, object>>? orderBy,
			IEnumerable<Expression<Func<TModel, bool>>>? filters,
			Expression<Func<TModel, TProjection>> constructionExpression,
			CancellationToken cancellationToken = default
		)
			where TModel : Entity
		{
			var result = await repository.GetPagedProjectionAsync(
				pageRequest.Next,
				pageRequest.Count,
				constructionExpression,
				descending,
				pageRequest.TotalCount == 0,
				orderBy,
				filters,
				cancellationToken
			);

			var updatedPage = new PageRequest(pageRequest.Next, pageRequest.Count, result.TotalCount);

			return new PagedResult<TProjection>(result.Items.ToList(), updatedPage);
		}

		public static Task<PagedResult<TProjection>> GetPagedProjectionsAsync<TProjection, TModel>(
			IRepository<TModel> repository,
			IEnumerable<FilterExpression> availableFilters,
			QueryRequest queryRequest,
			Expression<Func<TModel, TProjection>> constructionExpression,
			CancellationToken cancellationToken = default
		)
			where TModel : Entity
		{
			var expressionBuilder = new QueryExpressionBuilder(availableFilters);

			return GetPagedProjectionsByPageAsync(
				repository,
				queryRequest.PageRequest,
				queryRequest.Descending,
				expressionBuilder.GetOrderBy<TModel>(queryRequest),
				expressionBuilder.Build<TModel>(queryRequest.Filters),
				constructionExpression,
				cancellationToken
			);
		}

		public static async Task<PagedResult<TDto>> GetPagedAsync<TDto, TModel>(
			IRepository<TModel> repository,
			IEnumerable<FilterExpression> availableFilters,
			QueryRequest queryRequest,
			Func<TModel, TDto> modelToDtoTransformer,
			CancellationToken cancellationToken = default
		)
			where TModel : Entity
		{
			var expressionBuilder = new QueryExpressionBuilder(availableFilters);

			var result = await repository.GetMultiplePagedAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				queryRequest.Descending,
				queryRequest.PageRequest.TotalCount == 0,
				expressionBuilder.GetOrderBy<TModel>(queryRequest),
				expressionBuilder.Build<TModel>(queryRequest.Filters),
				cancellationToken
			);

			var updatedPage = new PageRequest(queryRequest.PageRequest.Next, queryRequest.PageRequest.Count, result.TotalCount);

			return new PagedResult<TDto>(result.Items.Select(modelToDtoTransformer).ToList(), updatedPage);
		}
	}
}