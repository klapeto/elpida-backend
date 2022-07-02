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

using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
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
	public class BenchmarkResultController
		: ServiceController<BenchmarkResultDto, BenchmarkResultPreviewDto, IBenchmarkResultService>
	{
		public BenchmarkResultController(IBenchmarkResultService benchmarkResultService)
			: base(benchmarkResultService)
		{
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
		[ApiKeyAuthentication("Results")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> PostNewResult(
			[FromBody] ResultBatchDto resultDto,
			CancellationToken cancellationToken
		)
		{
			var result = await Service.AddBatchAsync(resultDto, cancellationToken);
			return CreatedAtAction(nameof(GetSingle), new { id = result.First() }, null);
		}
	}
}