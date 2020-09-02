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
			if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be empty", nameof(id));
			
			var model = await _resultsRepository.GetSingleAsync(id, cancellationToken);
			if (model == null) throw new NotFoundException(id);
			
			return model.ToDto();
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