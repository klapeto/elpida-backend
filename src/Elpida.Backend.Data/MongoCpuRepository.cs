using System;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Elpida.Backend.Data
{
	public class MongoCpuRepository: ICpuRepository
	{
		private readonly IMongoCollection<CpuModel> _cpuCollection;
		private readonly IMongoCollection<TopologyModel> _topologyCollection;

		public MongoCpuRepository(IMongoCollection<CpuModel> cpuCollection, IMongoCollection<TopologyModel> topologyCollection)
		{
			_cpuCollection = cpuCollection;
			_topologyCollection = topologyCollection;
		}

		public Task<CpuModel> GetCpuByHashAsync(string hash, CancellationToken cancellationToken)
		{
			return _cpuCollection.AsQueryable().Where(m => m.Hash == hash).FirstOrDefaultAsync(cancellationToken: cancellationToken);
		}

		public Task<TopologyModel> GetTopologyByHashAsync(string hash, CancellationToken cancellationToken)
		{
			return _topologyCollection.AsQueryable().Where(m => m.Hash == hash).FirstOrDefaultAsync(cancellationToken: cancellationToken);
		}

		public Task CreateCpuAsync(CpuModel cpuModel, CancellationToken cancellationToken)
		{
			cpuModel.Id = ObjectId.GenerateNewId(DateTime.UtcNow).ToString();
			return _cpuCollection.InsertOneAsync(cpuModel, cancellationToken: cancellationToken);
		}

		public Task CreateTopologyAsync(TopologyModel topologyModel, CancellationToken cancellationToken)
		{
			topologyModel.Id = ObjectId.GenerateNewId(DateTime.UtcNow).ToString();
			return _topologyCollection.InsertOneAsync(topologyModel, cancellationToken: cancellationToken);
		}
	}
}