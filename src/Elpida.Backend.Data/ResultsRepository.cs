/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
	public class ResultsRepository : EntityRepository<ResultModel>, IResultsRepository
	{
		public ResultsRepository(ElpidaContext elpidaContext)
			: base(elpidaContext, elpidaContext.Results)
		{

		}
		
		protected override IQueryable<ResultModel> ProcessGetSingle(IQueryable<ResultModel> queryable)
		{
			return queryable
				.Include(model => model.Benchmark)
				.Include(model => model.TaskResults)
				.ThenInclude(model => model.Task)
				.Include(model => model.Topology)
				.ThenInclude(model => model.Cpu);
		}

		public async Task<PagedQueryResult<ResultPreviewModel>> GetPagedPreviewsAsync<TOrderKey>(
			int from,
			int count,
			bool descending,
			Expression<Func<ResultModel, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<ResultModel, bool>>> filters,
			bool calculateTotalCount,
			CancellationToken cancellationToken = default)
		{
			if (from < 0)
			{
				throw new ArgumentException("'from' must be positive or 0", nameof(from));
			}

			if (count <= 0)
			{
				throw new ArgumentException("'count' must be positive", nameof(count));
			}

			var result = Collection.AsQueryable();

			if (filters != null)
			{
				result = filters.Aggregate(result, (current, filter) => current.Where(filter));
			}

			if (orderBy != null)
			{
				result = descending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
			}

			var totalCount = calculateTotalCount ? await result.CountAsync(cancellationToken) : 0;

			var results = await result
				.Skip(from)
				.Take(count)
				.Include(model => model.Benchmark)
				.Include(model => model.Topology)
				.ThenInclude(model => model.Cpu)
				.Select(m => new ResultPreviewModel
				{
					Id = m.Id,
					Name = m.Benchmark.Name,
					CpuBrand = m.Topology.Cpu.Brand,
					CpuCores = m.Topology.TotalPhysicalCores,
					CpuFrequency = m.Topology.Cpu.Frequency,
					ElpidaVersion = m.ElpidaVersion,
					MemorySize = m.MemorySize,
					OsName = m.OsName,
					OsVersion = m.OsVersion,
					TimeStamp = m.TimeStamp,
					CpuLogicalCores = m.Topology.TotalLogicalCores
				})
				.ToListAsync(cancellationToken);

			return new PagedQueryResult<ResultPreviewModel>(totalCount, results);
		}
	}
}