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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Elpida.Backend.Data
{
	public class MongoCpuRepository : ICpuRepository
	{
		private readonly IMongoCollection<CpuModel> _cpuCollection;
		private readonly IMongoCollection<TopologyModel> _topologyCollection;

		public MongoCpuRepository(IMongoCollection<CpuModel> cpuCollection,
			IMongoCollection<TopologyModel> topologyCollection)
		{
			_cpuCollection = cpuCollection;
			_topologyCollection = topologyCollection;
		}

		#region ICpuRepository Members

		public Task<CpuModel> GetCpuByIdAsync(string id, CancellationToken cancellationToken)
		{
			return _cpuCollection.AsQueryable().Where(m => m.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		public Task<TopologyModel> GetTopologyByIdAsync(string id, CancellationToken cancellationToken)
		{
			return _topologyCollection.AsQueryable().Where(m => m.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		public Task CreateCpuAsync(CpuModel cpuModel, CancellationToken cancellationToken)
		{
			return _cpuCollection.InsertOneAsync(cpuModel, cancellationToken: cancellationToken);
		}

		public Task CreateTopologyAsync(TopologyModel topologyModel, CancellationToken cancellationToken)
		{
			return _topologyCollection.InsertOneAsync(topologyModel, cancellationToken: cancellationToken);
		}

		#endregion
	}
}