// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
	public interface IBenchmarkResultsService
	{
		Task<PagedResult<BenchmarkResultPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		);

		Task<BenchmarkResultDto> GetSingleAsync(long id, CancellationToken cancellationToken = default);

		Task<long> AddAsync(
			long cpuId,
			long topologyId,
			long osId,
			long elpidaId,
			BenchmarkResultSlimDto benchmarkResult,
			MemoryDto memory,
			TimingDto timing,
			CancellationToken cancellationToken = default
		);

		Task<IList<long>> AddBatchAsync(BenchmarkResultsBatchDto batch, CancellationToken cancellationToken = default);
	}
}