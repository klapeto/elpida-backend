using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data.Abstractions
{
	public interface IResultsRepository
	{
		Task<ResultModel> GetSingleAsync(string id, CancellationToken cancellationToken = default);

		Task<string> CreateAsync(ResultModel resultModel, CancellationToken cancellationToken = default);

		Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default);

		Task<List<ResultPreviewModel>> GetAsync(int from, int count, bool desc = false, CancellationToken cancellationToken = default);

		Task DeleteAllAsync(CancellationToken cancellationToken = default);
	}
}