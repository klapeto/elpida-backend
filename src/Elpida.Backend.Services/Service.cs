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
		where TDto : FoundationDto
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

		public Task<PagedResult<TDto>> GetPagedAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return QueryUtilities.GetPagedAsync(
				Repository,
				GetFilterExpressions(),
				queryRequest,
				ToDto,
				cancellationToken
			);
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

		public IEnumerable<FilterExpression> ConstructCustomFilters<T, TR>(Expression<Func<T, TR>> baseExpression)
		{
			return FiltersTransformer.ConstructCustomFilters(baseExpression, GetFilterExpressions());
		}

		protected static async Task<TFModel> GetOrAddForeignDto<TFDto, TFModel>(
			IRepository<TFModel> repository,
			IService<TFDto> service,
			TFDto dto,
			CancellationToken cancellationToken
		)
			where TFModel : Entity
			where TFDto : FoundationDto
		{
			var newDto = await service.GetOrAddAsync(dto, cancellationToken);
			return (await repository.GetSingleAsync(newDto.Id, cancellationToken))!;
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