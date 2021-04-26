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
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public class TaskStatisticsRepository : EntityRepository<TaskStatisticsModel>, ITaskStatisticsRepository
    {
        public TaskStatisticsRepository(ElpidaContext context)
            : base(context, context.TaskStatistics)
        {
        }

        protected override IQueryable<TaskStatisticsModel> ProcessGetSingle(IQueryable<TaskStatisticsModel> queryable)
        {
            return queryable
                .Include(m => m.Cpu)
                .Include(m => m.Topology)
                .Include(m => m.Task);
        }

        protected override IQueryable<TaskStatisticsModel> ProcessGetMultiplePaged(
            IQueryable<TaskStatisticsModel> queryable)
        {
            return ProcessGetSingle(queryable);
        }

        // public async Task<PagedQueryResult<TReturnEntity>> GetPagedProjectionByCpuAsync<TOrderKey, TReturnEntity>(
        //     int from,
        //     int count,
        //     Expression<Func<CpuModel, IEnumerable<TaskStatisticsModel>, TReturnEntity>> constructionExpression,
        //     bool descending = false,
        //     bool calculateTotalCount = false,
        //     Expression<Func<TaskStatisticsModel, TOrderKey>>? orderBy = null,
        //     IEnumerable<Expression<Func<TaskStatisticsModel, bool>>>? filters = null,
        //     CancellationToken cancellationToken = default)
        // {
        //     var (totalCount, query) = await PreprocessQueryAsync(
        //         ProcessGetMultiplePaged(Collection.AsQueryable()),
        //         from,
        //         count,
        //         descending,
        //         calculateTotalCount,
        //         orderBy,
        //         filters,
        //         cancellationToken);
        //     
        //     
        //     var 
        //     
        //     
        //     return new PagedQueryResult<TReturnEntity>(0, null);
        // }
    }
}