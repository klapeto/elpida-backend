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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public abstract class EntityRepositoryWithPreviews<TEntity, TPreviewEntity>
        : EntityRepository<TEntity>, IRepositoryWithPreviews<TEntity, TPreviewEntity> where TEntity : Entity
    {
        protected EntityRepositoryWithPreviews(DbContext context, DbSet<TEntity> collection) : base(context, collection)
        {
        }

        public Task<PagedQueryResult<TPreviewEntity>> GetPagedPreviewsAsync<TOrderKey>(
            int from,
            int count,
            bool descending = false,
            bool calculateTotalCount = false,
            Expression<Func<TEntity, TOrderKey>>? orderBy = null,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters = null,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionAsync(from, count, GetPreviewConstructionExpression(), descending,
                calculateTotalCount, orderBy, filters, cancellationToken);
        }

        protected abstract Expression<Func<TEntity, TPreviewEntity>> GetPreviewConstructionExpression();
    }
}