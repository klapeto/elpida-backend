using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Elpida.Backend.Data
{
	public class MongoResultsRepository : IResultsRepository
	{
		private readonly IMongoCollection<ResultModel> _resultCollection;

		public MongoResultsRepository(IElpidaDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			_resultCollection = database.GetCollection<ResultModel>(settings.ElpidaCollectionName);
		}

		#region IResultsRepository Members

		public async Task<ResultModel> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			return await (await _resultCollection.FindAsync(r => r.Id == id, cancellationToken: cancellationToken))
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<string> CreateAsync(ResultModel resultModel, CancellationToken cancellationToken)
		{
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