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
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class ResultController : ControllerBase
	{
		private readonly IResultsService _resultsService;

		public ResultController(IResultsService resultsService)
		{
			_resultsService = resultsService;
		}

		[HttpDelete]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ApiKeyAuthentication(KeyName = "Results")]
		public async Task<IActionResult> ClearResults(CancellationToken cancellationToken)
		{
			await _resultsService.ClearResultsAsync(cancellationToken);
			return Ok();
		}

		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PostNewResult([FromBody] ResultDto resultDto,
			CancellationToken cancellationToken)
		{
			var id = await _resultsService.CreateAsync(resultDto, cancellationToken);
			return Created(new Uri($"{Request.GetEncodedUrl()}{id}", UriKind.Absolute), null);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSingle([FromRoute] string id, CancellationToken cancellationToken)
		{
			var resultDto = await _resultsService.GetSingleAsync(id, cancellationToken);
			if (resultDto != null) return Ok(resultDto);
			return NotFound(id);
		}

		[HttpPost("search")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Produces("application/json")]
		[Consumes("application/json")]
		public async Task<IActionResult> Search([FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken)
		{
			return Ok(await _resultsService.GetPagedAsync(queryRequest, cancellationToken));
		}
	}
}