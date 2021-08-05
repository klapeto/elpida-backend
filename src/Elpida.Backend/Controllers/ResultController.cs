/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2020 Ioannis Panagiotopoulos
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

using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	/// <summary>
	///     Controller for accessing Benchmark Results.
	/// </summary>
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ResultController : ControllerBase
	{
		private readonly IBenchmarkResultsService _benchmarkResultsService;

		public ResultController(IBenchmarkResultsService benchmarkResultsService)
		{
			_benchmarkResultsService = benchmarkResultsService;
		}

		/// <summary>
		///     Creates a new Benchmark Result.
		/// </summary>
		/// <param name="resultDto">The result data.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The url to access the created result.</returns>
		/// <response code="201">The result was successfully created.</response>
		/// <response code="400">The result data was invalid.</response>
		/// <response code="401">The client is unauthorized to create results.</response>
		[HttpPost]
		[ApiKeyAuthentication(KeyName = "Results")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> PostNewResult(
			[FromBody] ResultDto resultDto,
			CancellationToken cancellationToken
		)
		{
			var result = await _benchmarkResultsService.GetOrAddAsync(resultDto, cancellationToken);
			return CreatedAtRoute(nameof(GetSingleResult), new { id = result.Id, version = "v1" }, null);
		}

		/// <summary>
		///     Get all the Benchmark Results with paging.
		/// </summary>
		/// <param name="pageRequest">The page request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Benchmark Results requested.</returns>
		/// <response code="200">The returned page of the Benchmark Results previews.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<ResultPreviewDto>> GetPaged(
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return _benchmarkResultsService.GetPagedPreviewsAsync(
				new QueryRequest { PageRequest = pageRequest },
				cancellationToken
			);
		}

		/// <summary>
		///     Get the full details of a single Benchmark Result.
		/// </summary>
		/// <param name="id">The id of the Benchmark Result to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The Benchmark Result details.</returns>
		/// <response code="200">The returned data of the Benchmark Result.</response>
		/// <response code="404">The Benchmark Result with this id was not found.</response>
		[HttpGet("{id:long}", Name = nameof(GetSingleResult))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public Task<ResultDto> GetSingleResult([FromRoute] long id, CancellationToken cancellationToken)
		{
			return _benchmarkResultsService.GetSingleAsync(id, cancellationToken);
		}

		/// <summary>
		///     Search for Benchmark Results with the provided criteria.
		/// </summary>
		/// <param name="queryRequest">The query request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Benchmark Result previews that the search yielded.</returns>
		/// <response code="200">The returned page of the Benchmark Result previews that the search yielded.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpPost("Search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<ResultPreviewDto>> Search(
			[FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken
		)
		{
			QueryRequestUtilities.PreprocessQuery(queryRequest);

			return _benchmarkResultsService.GetPagedPreviewsAsync(queryRequest, cancellationToken);
		}
	}
}