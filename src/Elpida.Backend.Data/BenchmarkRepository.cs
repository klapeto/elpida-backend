/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
	public class BenchmarkRepository : EntityRepository<BenchmarkModel>, IBenchmarkRepository
	{
		public BenchmarkRepository(ElpidaContext elpidaContext)
			: base(elpidaContext, elpidaContext.Benchmarks)
		{
		}

		protected override IQueryable<BenchmarkModel> ProcessGetSingle(IQueryable<BenchmarkModel> queryable)
		{
			return queryable
				.Include(model => model.Tasks);
		}

		protected override IQueryable<BenchmarkModel> ProcessGetMultiple(IQueryable<BenchmarkModel> queryable)
		{
			return ProcessGetSingle(queryable);
		}

		protected override IQueryable<BenchmarkModel> ProcessGetMultiplePaged(IQueryable<BenchmarkModel> queryable)
		{
			return ProcessGetSingle(queryable);
		}
	}
}