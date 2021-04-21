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

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public abstract class ServiceWithPreviews<TDto, TModel, TPreviewDto, TPreviewModel> :
        Service<TDto, TModel, IRepositoryWithPreviews<TModel, TPreviewModel>>, IServiceWithPreviews<TDto, TPreviewDto>
        where TModel : Entity
    {
        protected ServiceWithPreviews(IRepositoryWithPreviews<TModel, TPreviewModel> repository)
            : base(repository)
        {
        }

        public async Task<PagedResult<TPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            var expressionBuilder = new QueryExpressionBuilder(GetLambdaFilters());

            var result = await Repository.GetPagedPreviewsAsync(
                queryRequest.PageRequest.Next,
                queryRequest.PageRequest.Count,
                queryRequest.Descending,
                queryRequest.PageRequest.TotalCount == 0,
                expressionBuilder.GetOrderBy<TModel>(queryRequest),
                expressionBuilder.Build<TModel>(queryRequest.Filters),
                cancellationToken);

            queryRequest.PageRequest.TotalCount = result.TotalCount;

            return new PagedResult<TPreviewDto>(result.Items.Select(ToPreviewDto).ToList(), queryRequest.PageRequest);
        }

        protected abstract TPreviewDto ToPreviewDto(TPreviewModel model);
    }
}