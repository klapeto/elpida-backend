using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private readonly IResultsRepository _resultsRepository;

		public ResultService(IResultsRepository resultsRepository)
		{
			_resultsRepository = resultsRepository ?? throw new ArgumentNullException(nameof(resultsRepository));
		}

		#region IResultsService Members

		public Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			if (resultDto == null) throw new ArgumentNullException(nameof(resultDto));
			resultDto.TimeStamp = DateTime.UtcNow;
			return _resultsRepository.CreateAsync(resultDto.ToModel(), cancellationToken);
		}

		public async Task<ResultDto> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id is empty", nameof(id));
			return (await _resultsRepository.GetSingleAsync(id, cancellationToken)).ToDto();
		}

		public async Task<PagedResult<ResultPreviewDto>> GetPagedAsync(PageRequest pageRequest,
			CancellationToken cancellationToken)
		{
			if (pageRequest == null) throw new ArgumentNullException(nameof(pageRequest));
			if (pageRequest.TotalCount == 0)
				pageRequest.TotalCount = await _resultsRepository.GetTotalCountAsync(cancellationToken);

			var list = (await _resultsRepository.GetAsync(pageRequest.Next, pageRequest.Count, true, cancellationToken))
				.Select(m => m.ToDto())
				.ToList();
			return new PagedResult<ResultPreviewDto>(list, pageRequest);
		}

		public Task ClearResultsAsync(CancellationToken cancellationToken)
		{
			return _resultsRepository.DeleteAllAsync(cancellationToken);
		}

		#endregion
	}
}