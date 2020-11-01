using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions
{
	public interface ICpuRepository
	{
		Task<CpuModel> GetCpuByHashAsync(string hash, CancellationToken cancellationToken);
		
		Task<TopologyModel> GetTopologyByHashAsync(string hash, CancellationToken cancellationToken);

		Task CreateCpuAsync(CpuModel cpuModel, CancellationToken cancellationToken);
		
		Task CreateTopologyAsync(TopologyModel topologyModel, CancellationToken cancellationToken);
	}
}