/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	/// <summary>
	///     Controller for accessing Benchmarks.
	/// </summary>
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BenchmarkController : ControllerBase
	{
		private readonly IBenchmarkService _benchmarkService;

		public BenchmarkController(IBenchmarkService benchmarkService)
		{
			_benchmarkService = benchmarkService;
		}

		/// <summary>
		///     Get all the Benchmark previews with paging.
		/// </summary>
		/// <param name="pageRequest">The page request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Benchmarks requested.</returns>
		/// <response code="200">The returned page of the Benchmark previews.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<BenchmarkPreviewDto>> GetPaged(
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return _benchmarkService.GetPagedPreviewsAsync(
				new QueryRequest { PageRequest = pageRequest },
				cancellationToken
			);
		}

		/// <summary>
		///     Get the full details of a single Benchmark.
		/// </summary>
		/// <param name="id">The id of the Benchmark to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The Benchmark details.</returns>
		/// <response code="200">The returned data of the Benchmark.</response>
		/// <response code="404">The Benchmark with this id was not found.</response>
		[HttpGet("{id:long}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public Task<BenchmarkDto> GetSingle([FromRoute] long id, CancellationToken cancellationToken)
		{
			return _benchmarkService.GetSingleAsync(id, cancellationToken);
		}

		/// <summary>
		///     Search for Benchmarks with the provided criteria.
		/// </summary>
		/// <param name="queryRequest">The query request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Benchmark previews that the search yielded.</returns>
		/// <response code="200">The returned page of the Benchmark previews that the search yielded.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpPost("Search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<BenchmarkPreviewDto>> Search(
			[FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken
		)
		{
			QueryRequestUtilities.PreprocessQuery(queryRequest);

			return _benchmarkService.GetPagedPreviewsAsync(queryRequest, cancellationToken);
		}
	}
}