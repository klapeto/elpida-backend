using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
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
		private IResultsService _resultsService;

		public ResultController(IResultsService resultsService)
		{
			_resultsService = resultsService;
		}

		[HttpDelete]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ClearResults(CancellationToken cancellationToken)
		{
			await _resultsService.ClearResultsAsync(cancellationToken);
			return Ok();
		}

		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PostNewResult([FromBody] ResultDto resultDto, CancellationToken cancellationToken)
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

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetPaged([FromQuery] PageRequest pageRequest, CancellationToken cancellationToken)
		{
			return Ok(await _resultsService.GetPagedAsync(pageRequest, cancellationToken));
		}
	}
}