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
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Elpida.Backend.Data
{
	public class MongoResultsRepository : MongoRepository<ResultModel>, IResultsRepository
	{
		private readonly IMongoCollection<CpuModel> _cpuCollection;
		private readonly IMongoCollection<TopologyModel> _topologyCollection;

		public MongoResultsRepository(
			IMongoCollection<ResultModel> resultCollection,
			IMongoCollection<CpuModel> cpuCollection,
			IMongoCollection<TopologyModel> topologyCollection)
			: base(resultCollection)
		{
			_cpuCollection = cpuCollection ?? throw new ArgumentNullException(nameof(cpuCollection));
			_topologyCollection = topologyCollection ?? throw new ArgumentNullException(nameof(topologyCollection));
		}

		public Task<ResultProjection> GetProjectionAsync(string id, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("'Id' cannot be empty", nameof(id));
			}

			return JoinCollectionData().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
		}

		public async Task<PagedQueryResult<ResultPreviewModel>> GetPagedPreviewsAsync<TOrderKey>(
			int from,
			int count,
			bool descending,
			Expression<Func<ResultProjection, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<ResultProjection, bool>>> filters,
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

			var result = JoinCollectionData();

			if (filters != null)
			{
				result = filters.Aggregate(result, (current, filter) => current.Where(filter));
			}
			
			if (orderBy != null)
			{
				result = descending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
			}

			var totalCount = calculateTotalCount ? await result.CountAsync(cancellationToken) : 0;

			var results = await result.Skip(from)
				.Take(count)
				.Select(m => new ResultPreviewModel
				{
					CpuBrand = m.System.Cpu.Brand,
					CpuCores = m.System.Topology.TotalPhysicalCores,
					CpuLogicalCores = m.System.Topology.TotalLogicalCores,
					CpuFrequency = m.System.Cpu.Frequency,
					Name = m.Result.Name,
					Id = m.Id,
					OsName = m.System.Os.Name,
					OsVersion = m.System.Os.Version,
					ElpidaVersionMajor = m.Elpida.Version.Major,
					ElpidaVersionMinor = m.Elpida.Version.Minor,
					ElpidaVersionRevision = m.Elpida.Version.Revision,
					ElpidaVersionBuild = m.Elpida.Version.Build,
					MemorySize = m.System.Memory.TotalSize,
					TimeStamp = m.TimeStamp
				})
				.ToListAsync(cancellationToken);

			return new PagedQueryResult<ResultPreviewModel>(totalCount, results);
		}

		private IMongoQueryable<ResultProjection> JoinCollectionData()
		{
			return Collection.AsQueryable()
				.Join(_cpuCollection.AsQueryable(),
					rModel => rModel.System.CpuId,
					cModel => cModel.Id,
					(model, cpuModel) => new {ResultModel = model, CpuModel = cpuModel})
				.Join(_topologyCollection.AsQueryable(),
					t => t.ResultModel.System.TopologyId,
					topology => topology.Id, (t, topology) => new ResultProjection
					{
						Affinity = t.ResultModel.Affinity,
						Elpida = t.ResultModel.Elpida,
						Id = t.ResultModel.Id,
						Result = t.ResultModel.Result,
						System = new SystemModelProjection
						{
							Cpu = t.CpuModel,
							Memory = t.ResultModel.System.Memory,
							Os = t.ResultModel.System.Os,
							Timing = t.ResultModel.System.Timing,
							Topology = topology
						},
						TimeStamp = t.ResultModel.TimeStamp
					});
		}
	}
}