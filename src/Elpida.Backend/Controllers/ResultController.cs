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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class ResultController : ControllerBase
	{
		private readonly IBenchmarkResultsService _benchmarkResultsService;

		public ResultController(IBenchmarkResultsService benchmarkResultsService)
		{
			_benchmarkResultsService = benchmarkResultsService;
		}

		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PostNewResult([FromBody] ResultDto resultDto, ApiVersion apiVersion,
			CancellationToken cancellationToken)
		{
			var result = await _benchmarkResultsService.GetOrAddAsync(resultDto, cancellationToken);
			return CreatedAtRoute(nameof(GetSingle), new {id = result.Id, version = $"{apiVersion}"}, null);
		}

		[HttpGet("{id}", Name = nameof(GetSingle))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSingle([FromRoute] string id, CancellationToken cancellationToken)
		{
			if (long.TryParse(id, out var lid))
			{
				return Ok(await _benchmarkResultsService.GetSingleAsync(lid, cancellationToken));
			}

			return BadRequest("Id must be an Integer number");
		}

		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetPaged([FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken)
		{
			return Ok(await _benchmarkResultsService.GetPagedPreviewsAsync(new QueryRequest {PageRequest = pageRequest},
				cancellationToken));
		}

		[HttpPost("search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Search([FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken)
		{
			if (queryRequest.Filters != null)
			{
				foreach (var queryRequestFilter in queryRequest.Filters)
				{
					ConvertValues(queryRequestFilter);
				}
			}

			return Ok(await _benchmarkResultsService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
		}

		private static void ConvertValues(QueryInstance instance)
		{
			var element = (JsonElement) instance.Value;
			switch (element.ValueKind)
			{
				case JsonValueKind.String:
					instance.Value = element.GetString();
					break;
				case JsonValueKind.Number:
					instance.Value = element.GetDouble();
					break;
				case JsonValueKind.False:
				case JsonValueKind.True:
					instance.Value = element.GetBoolean();
					break;
				case JsonValueKind.Null:
					instance.Value = null;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}