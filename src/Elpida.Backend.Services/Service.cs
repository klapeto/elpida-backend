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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
	public abstract class Service<TDto, TPreview, TModel, TRepository> : IService<TDto, TPreview>
		where TModel : Entity
		where TRepository : IRepository<TModel>
		where TDto : FoundationDto
		where TPreview : FoundationDto
	{
		protected Service(TRepository repository)
		{
			Repository = repository;
		}

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

		public Task<PagedResult<TPreview>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return QueryUtilities.GetPagedProjectionsAsync(
				Repository,
				GetFilterExpressions(),
				queryRequest,
				GetPreviewConstructionExpression(),
				cancellationToken
			);
		}

		public virtual async Task<TDto> GetOrAddAsync(TDto dto, CancellationToken cancellationToken = default)
		{
			var bypassExpression = GetCreationBypassCheckExpression(dto);
			if (bypassExpression == null)
			{
				return await DoAddAsync(dto, null, cancellationToken);
			}

			using var transaction = await Repository.BeginTransactionAsync(cancellationToken);

			var entity = await Repository.GetSingleAsync(bypassExpression, cancellationToken);
			if (entity != null)
			{
				return ToDto(entity);
			}

			return await DoAddAsync(dto, transaction, cancellationToken);
		}

		public IEnumerable<FilterExpression> ConstructCustomFilters<T, TR>(Expression<Func<T, TR>> baseExpression)
		{
			return FiltersTransformer.ConstructCustomFilters(baseExpression, GetFilterExpressions());
		}

		protected abstract TDto ToDto(TModel model);

		protected abstract Expression<Func<TModel, TPreview>> GetPreviewConstructionExpression();

		protected abstract Task<TModel> ProcessDtoAndCreateModelAsync(TDto dto, CancellationToken cancellationToken);

		protected virtual IEnumerable<FilterExpression> GetFilterExpressions()
		{
			yield break;
		}

		protected virtual Expression<Func<TModel, bool>>? GetCreationBypassCheckExpression(TDto dto)
		{
			return null;
		}

		private async Task<TDto> DoAddAsync(TDto dto, ITransaction? transaction, CancellationToken cancellationToken)
		{
			var entity = await ProcessDtoAndCreateModelAsync(dto, cancellationToken);
			entity.Id = 0;
			entity = await Repository.CreateAsync(entity, cancellationToken);

			await Repository.SaveChangesAsync(cancellationToken);

			if (transaction != null)
			{
				await transaction.CommitAsync(cancellationToken);
			}

			return ToDto(entity);
		}
	}
}