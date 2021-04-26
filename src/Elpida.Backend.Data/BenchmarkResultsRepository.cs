/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020 Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public class BenchmarkResultsRepository :
        EntityRepository<BenchmarkResultModel>,
        IBenchmarkResultsRepository
    {
        public BenchmarkResultsRepository(ElpidaContext elpidaContext)
            : base(elpidaContext, elpidaContext.BenchmarkResults)
        {
        }

        protected override IQueryable<BenchmarkResultModel> ProcessGetMultiplePaged(
            IQueryable<BenchmarkResultModel> queryable)
        {
            return ProcessGetSingle(queryable);
        }

        protected override IQueryable<BenchmarkResultModel> ProcessGetSingle(IQueryable<BenchmarkResultModel> queryable)
        {
            return queryable
                .AsNoTracking()
                .Include(model => model.Benchmark)
                .Include(model => model.Os)
                .Include(model => model.Elpida)
                .Include(model => model.TaskResults)
                .ThenInclude(model => model.Task)
                .Include(model => model.Topology)
                .ThenInclude(model => model.Cpu);
        }
    }
}