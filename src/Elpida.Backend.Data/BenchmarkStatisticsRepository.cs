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
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
	public class BenchmarkStatisticsRepository
		: EntityRepository<BenchmarkStatisticsModel>, IBenchmarkStatisticsRepository
	{
		public BenchmarkStatisticsRepository(ElpidaContext context)
			: base(context, context.BenchmarkStatistics)
		{
		}

		protected override IQueryable<BenchmarkStatisticsModel> ProcessGetSingle(
			IQueryable<BenchmarkStatisticsModel> queryable
		)
		{
			return queryable
				.Include(m => m.Cpu)
				.Include(m => m.Topology)
				.Include(m => m.Benchmark);
		}

		protected override IQueryable<BenchmarkStatisticsModel> ProcessGetMultiplePaged(
			IQueryable<BenchmarkStatisticsModel> queryable
		)
		{
			return ProcessGetSingle(queryable);
		}
	}
}