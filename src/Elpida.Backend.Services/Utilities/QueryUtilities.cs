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
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
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

		public static async Task<TModel> GetOrAddSafeAsync<TModel>(
			IRepository<TModel> repository,
			TModel model,
			Expression<Func<TModel, bool>> searchExpression,
			CancellationToken cancellationToken
		)
			where TModel : Entity
		{
			var entity = await repository.GetSingleAsync(searchExpression, cancellationToken);
			if (entity != null)
			{
				return entity;
			}

			try
			{
				entity = model;
				entity.Id = 0;
				entity = await repository.CreateAsync(entity, cancellationToken);

				await repository.SaveChangesAsync(cancellationToken);
				return entity;
			}
			catch (DuplicateRecordException)
			{
				await repository.DropAddedAsync(entity!, cancellationToken);
				await repository.SaveChangesAsync(cancellationToken); // Safe synchronization

				entity = await repository.GetSingleAsync(searchExpression, cancellationToken);

				if (entity != null)
				{
					return entity;
				}

				throw;
			}
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
	}
}