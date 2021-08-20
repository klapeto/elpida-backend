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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Statistics;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	/// <summary>
	///     Controller for accessing benchmark/cpu statistics.
	/// </summary>
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BenchmarkStatisticsController : ControllerBase
	{
		private readonly IBenchmarkStatisticsService _benchmarkStatisticsService;

		public BenchmarkStatisticsController(IBenchmarkStatisticsService benchmarkStatisticsService)
		{
			_benchmarkStatisticsService = benchmarkStatisticsService;
		}

		/// <summary>
		///     Get all the benchmark/cpu statistics with paging.
		/// </summary>
		/// <param name="pageRequest">The page request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of benchmark/cpu statistics requested.</returns>
		/// <response code="200">The returned page of the benchmark/cpu statistics previews.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<BenchmarkStatisticsPreviewDto>> GetPaged(
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return _benchmarkStatisticsService.GetPagedPreviewsAsync(
				new QueryRequest { PageRequest = pageRequest },
				cancellationToken
			);
		}

		/// <summary>
		///     Get the full details of a single benchmark/cpu statistic.
		/// </summary>
		/// <param name="id">The id of the benchmark/cpu statistic to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The benchmark/cpu statistics details.</returns>
		/// <response code="200">The returned data of the benchmark/cpu statistics.</response>
		/// <response code="404">The benchmark/cpu statistics with this id was not found.</response>
		[HttpGet("{id:long}", Name = nameof(GetSingleStatistic))]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public Task<BenchmarkStatisticsDto> GetSingleStatistic(
			[FromRoute] long id,
			CancellationToken cancellationToken
		)
		{
			return _benchmarkStatisticsService.GetSingleAsync(id, cancellationToken);
		}

		/// <summary>
		///     Search for benchmark/cpu statistics with the provided criteria.
		/// </summary>
		/// <param name="queryRequest">The query request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of benchmark/cpu statistics previews that the search yielded.</returns>
		/// <response code="200">The returned page of the benchmark/cpu statistics previews that the search yielded.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpPost("Search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<BenchmarkStatisticsPreviewDto>> Search(
			[FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken
		)
		{
			QueryRequestUtilities.PreprocessQuery(queryRequest);

			return _benchmarkStatisticsService.GetPagedPreviewsAsync(queryRequest, cancellationToken);
		}
	}
}