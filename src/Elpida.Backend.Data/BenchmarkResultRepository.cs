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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
	public class BenchmarkResultRepository
		: EntityRepository<BenchmarkResultModel>,
			IBenchmarkResultRepository
	{
		public BenchmarkResultRepository(ElpidaContext elpidaContext)
			: base(elpidaContext, elpidaContext.BenchmarkResults)
		{
		}

		public Task<long> GetCountWithScoreBetween(
			long benchmarkId,
			long cpuId,
			double min,
			double max,
			CancellationToken cancellationToken = default
		)
		{
			return Collection
				.Where(
					s => s.Benchmark.Id == benchmarkId
					     && s.Topology.Cpu.Id == cpuId
					     && s.Score >= min
					     && s.Score < max
				)
				.LongCountAsync(cancellationToken);
		}

		public async Task<BasicStatisticsModel> GetStatisticsAsync(
			long benchmarkId,
			long cpuId,
			CancellationToken cancellationToken = default
		)
		{
			var baseQuery = Collection
				.AsNoTracking()
				.Where(m => m.Topology.Cpu.Id == cpuId && m.Benchmark.Id == benchmarkId)
				.GroupBy(m => m.Benchmark.Id);

			var result = await baseQuery
				.Select(
					m => new BasicStatisticsModel
					{
						Mean = m.Average(x => x.Score),
						Max = m.Max(x => x.Score),
						Min = m.Min(x => x.Score),
						Count = m.LongCount(),
					}
				)
				.FirstAsync(cancellationToken);

			if (result.Count == 0)
			{
				return result;
			}

			var variance = await baseQuery
				               .Select(m => m.Sum(x => (x.Score - result.Mean) * (x.Score - result.Mean)))
				               .FirstAsync(cancellationToken)
			               / result.Count;

			result.StandardDeviation = Math.Sqrt(variance);
			result.MarginOfError = result.StandardDeviation / Math.Sqrt(result.Count);

			return result;
		}

		protected override IQueryable<BenchmarkResultModel> ProcessGetMultiplePaged(
			IQueryable<BenchmarkResultModel> queryable
		)
		{
			return ProcessGetSingle(queryable);
		}

		protected override IQueryable<BenchmarkResultModel> ProcessGetSingle(IQueryable<BenchmarkResultModel> queryable)
		{
			return queryable
				.AsNoTracking()
				.Include(model => model.Benchmark)
				.Include(model => model.OperatingSystem)
				.Include(model => model.ElpidaVersion)
				.Include(model => model.TaskResults)
				.ThenInclude(model => model.Task)
				.Include(model => model.Topology)
				.ThenInclude(model => model.Cpu);
		}
	}
}