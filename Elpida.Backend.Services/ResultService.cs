﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services
{
	public class ResultService : IResultsService
	{
		private readonly IResultsRepository _resultsRepository;

		public ResultService(IResultsRepository resultsRepository)
		{
			_resultsRepository = resultsRepository;
		}

		#region IResultsService Members

		public Task<string> CreateAsync(ResultDto resultDto, CancellationToken cancellationToken)
		{
			resultDto.TimeStamp = DateTimeOffset.UtcNow;
			return _resultsRepository.CreateAsync(resultDto.ToResultModel(), cancellationToken);
		}

		public async Task<ResultDto> GetSingleAsync(string id, CancellationToken cancellationToken)
		{
			return (await _resultsRepository.GetSingleAsync(id, cancellationToken)).ToResultDto();
		}

		public async Task<PagedResult<ResultPreviewDto>> GetPagedAsync(PageRequest pageRequest,
			CancellationToken cancellationToken)
		{
			if (pageRequest.TotalCount == 0)
				pageRequest.TotalCount = await _resultsRepository.GetTotalCountAsync(cancellationToken);

			var list = (await _resultsRepository.GetAsync(pageRequest.Next, pageRequest.Count, true, cancellationToken))
				.Select(m => m.ToPreviewDto())
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