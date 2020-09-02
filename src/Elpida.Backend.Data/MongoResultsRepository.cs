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
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Elpida.Backend.Data
{
	public class MongoResultsRepository : IResultsRepository
	{
		private readonly IMongoCollection<ResultModel> _resultCollection;

		public MongoResultsRepository(IMongoCollection<ResultModel> resultCollection)
		{
			_resultCollection = resultCollection ?? throw new ArgumentNullException(nameof(resultCollection));
		}

		#region IResultsRepository Members

		public async Task<ResultModel> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("'Id' cannot be empty", nameof(id));

			return await (await _resultCollection.FindAsync(r => r.Id == id, cancellationToken: cancellationToken))
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<string> CreateAsync(ResultModel resultModel, CancellationToken cancellationToken)
		{
			if (resultModel == null) throw new ArgumentNullException(nameof(resultModel));

			resultModel.Id = ObjectId.GenerateNewId(DateTime.UtcNow).ToString();
			await _resultCollection.InsertOneAsync(resultModel, cancellationToken: cancellationToken);
			return resultModel.Id;
		}

		public Task<long> GetTotalCountAsync(CancellationToken cancellationToken)
		{
			return _resultCollection.CountDocumentsAsync(FilterDefinition<ResultModel>.Empty,
				cancellationToken: cancellationToken);
		}

		public Task<List<ResultPreviewModel>> GetAsync(int from, int count, bool desc,
			CancellationToken cancellationToken)
		{
			if (from < 0) throw new ArgumentException("'from' must be positive or 0", nameof(from));
			if (count <= 0) throw new ArgumentException("'count' must be positive", nameof(count));

			var result = _resultCollection.AsQueryable();

			if (desc) result = result.OrderByDescending(m => m.TimeStamp);

			return result.Skip(from)
				.Take(count)
				.Select(m => new ResultPreviewModel
				{
					Name = m.Result.Name,
					Id = m.Id,
					OsName = m.System.Os.Name,
					OsVersion = m.System.Os.Version,
					ElpidaVersionMajor = m.Elpida.Version.Major,
					ElpidaVersionMinor = m.Elpida.Version.Minor,
					ElpidaVersionRevision = m.Elpida.Version.Revision,
					ElpidaVersionBuild = m.Elpida.Version.Build,
					CpuBrand = m.System.Cpu.Brand,
					CpuCores = m.System.Topology.TotalPhysicalCores,
					CpuLogicalCores = m.System.Topology.TotalLogicalCores,
					CpuFrequency = m.System.Cpu.Frequency,
					MemorySize = m.System.Memory.TotalSize,
					TimeStamp = m.TimeStamp
				})
				.ToListAsync(cancellationToken);
		}

		public Task DeleteAllAsync(CancellationToken cancellationToken)
		{
			return _resultCollection.DeleteManyAsync(FilterDefinition<ResultModel>.Empty, cancellationToken);
		}

		#endregion
	}
}