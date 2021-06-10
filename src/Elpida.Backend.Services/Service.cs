/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
	public abstract class Service<TDto, TModel, TRepository> : IService<TDto>
		where TModel : Entity
		where TRepository : IRepository<TModel>
		where TDto : FountationDto
	{
		protected Service(TRepository repository, ILockFactory lockFactory)
		{
			Repository = repository;
			LockFactory = lockFactory;
		}

		protected ILockFactory LockFactory { get; }

		protected TRepository Repository { get; }

		public async Task<TDto> GetSingleAsync(long id, CancellationToken cancellationToken = default)
		{
			var entity = await Repository.GetSingleAsync(id, cancellationToken);

			if (entity == null)
			{
				throw new NotFoundException($"{typeof(TDto).Name} was not found", id);
			}

			return ToDto(entity);
		}

		public async Task<PagedResult<TDto>> GetPagedAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			var expressionBuilder = new QueryExpressionBuilder(GetLambdaFilters());

			var result = await Repository.GetMultiplePagedAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				queryRequest.Descending,
				queryRequest.PageRequest.TotalCount == 0,
				expressionBuilder.GetOrderBy<TModel>(queryRequest),
				expressionBuilder.Build<TModel>(queryRequest.Filters),
				cancellationToken
			);

			queryRequest.PageRequest.TotalCount = result.TotalCount;

			return new PagedResult<TDto>(result.Items.Select(ToDto).ToList(), queryRequest.PageRequest);
		}

		public virtual async Task<TDto> GetOrAddAsync(TDto dto, CancellationToken cancellationToken = default)
		{
			var bypassExpression = GetCreationBypassCheckExpression(dto);
			if (bypassExpression == null)
			{
				return await DoAddAsync(dto, cancellationToken);
			}

			var entity = await Repository.GetSingleAsync(bypassExpression, cancellationToken);
			if (entity != null)
			{
				return ToDto(entity);
			}

			using (await LockFactory.AcquireAsync(GetType().Name, cancellationToken))
			{
				entity = await Repository.GetSingleAsync(bypassExpression, cancellationToken);
				if (entity != null)
				{
					return ToDto(entity);
				}

				return await DoAddAsync(dto, cancellationToken);
			}
		}

		public IEnumerable<FilterExpression> GetFilters<T, TR>(Expression<Func<T, TR>> baseExpression)
		{
			if (typeof(TR) != typeof(TModel))
			{
				throw new ArgumentException($"The type of the TR is not of type: {typeof(TModel).Name}");
			}

			var baseBody = baseExpression.Body;
			foreach (var filter in GetFilterExpressions())
			{
				yield return new FilterExpression(
					filter.Name,
					Expression.MakeMemberAccess(baseBody, filter.Expression.Member)
				);
			}
		}

		protected static FilterExpression CreateFilter<T>(string name, Expression<Func<TModel, T>> expression)
		{
			if (expression.Body.NodeType != ExpressionType.MemberAccess)
			{
				throw new InvalidOperationException("The expression body must be member access of the model");
			}

			return new FilterExpression(name.ToLowerInvariant(), (MemberExpression)expression.Body);
		}

		protected static async Task<TFModel> GetOrAddForeignDto<TFDto, TFModel>(
			IRepository<TFModel> repository,
			IService<TFDto> service,
			TFDto dto,
			CancellationToken cancellationToken
		)
			where TFModel : Entity
			where TFDto : FountationDto
		{
			var newDto = await service.GetOrAddAsync(dto, cancellationToken);
			return (await repository.GetSingleAsync(newDto.Id, cancellationToken))!;
		}

		protected IReadOnlyDictionary<string, LambdaExpression> GetLambdaFilters()
		{
			return GetFilterExpressions()
				.ToDictionary(
					p => p.Name,
					p => Expression.Lambda(p.Expression, GetParameterExpression(p.Expression))
				);
		}

		protected abstract TDto ToDto(TModel model);

		protected abstract Task<TModel> ProcessDtoAndCreateModelAsync(TDto dto, CancellationToken cancellationToken);

		protected virtual IEnumerable<FilterExpression> GetFilterExpressions()
		{
			yield break;
		}

		protected virtual Expression<Func<TModel, bool>>? GetCreationBypassCheckExpression(TDto dto)
		{
			return null;
		}

		protected virtual Task OnEntityCreatedAsync(TDto dto, TModel entity, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		protected async Task<PagedResult<TProjection>> GetPagedProjectionsAsync<TProjection>(
			QueryRequest queryRequest,
			Expression<Func<TModel, TProjection>> constructionExpression,
			CancellationToken cancellationToken = default
		)
		{
			var expressionBuilder = new QueryExpressionBuilder(GetLambdaFilters());

			var result = await Repository.GetPagedProjectionAsync(
				queryRequest.PageRequest.Next,
				queryRequest.PageRequest.Count,
				constructionExpression,
				queryRequest.Descending,
				queryRequest.PageRequest.TotalCount == 0,
				expressionBuilder.GetOrderBy<TModel>(queryRequest),
				expressionBuilder.Build<TModel>(queryRequest.Filters),
				cancellationToken
			);

			queryRequest.PageRequest.TotalCount = result.TotalCount;

			return new PagedResult<TProjection>(result.Items.ToList(), queryRequest.PageRequest);
		}

		private static ParameterExpression GetParameterExpression(Expression expression)
		{
			while (expression.NodeType == ExpressionType.MemberAccess)
			{
				expression = ((MemberExpression)expression).Expression;
			}

			if (expression.NodeType != ExpressionType.Parameter)
			{
				throw new ArgumentException("Expression does not contain parameter");
			}

			return (ParameterExpression)expression;
		}

		private async Task<TDto> DoAddAsync(TDto dto, CancellationToken cancellationToken)
		{
			var entity = await ProcessDtoAndCreateModelAsync(dto, cancellationToken);
			entity.Id = 0;
			entity = await Repository.CreateAsync(entity, cancellationToken);

			await Repository.SaveChangesAsync(cancellationToken);

			dto = ToDto(entity);

			await OnEntityCreatedAsync(dto, entity, cancellationToken);

			return dto;
		}
	}
}