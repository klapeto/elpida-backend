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
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Statistics;

namespace Elpida.Backend.Data.Abstractions.Repositories
{
    public interface ITaskStatisticsRepository : IRepository<TaskStatisticsModel>
    {
        // Task<PagedQueryResult<TReturnEntity>> GetPagedProjectionByCpuAsync<TOrderKey, TReturnEntity>(
        //     int from,
        //     int count,
        //     Expression<Func<CpuModel, IEnumerable<TaskStatisticsModel>, TReturnEntity>> constructionExpression,
        //     bool descending = false,
        //     bool calculateTotalCount = false,
        //     Expression<Func<TaskStatisticsModel, TOrderKey>>? orderBy = null,
        //     IEnumerable<Expression<Func<TaskStatisticsModel, bool>>>? filters = null,
        //     CancellationToken cancellationToken = default);
    }
}