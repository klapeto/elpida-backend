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
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	/// <summary>
	///     Controller for accessing Elpida versions.
	/// </summary>
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ElpidaController : ControllerBase
	{
		private readonly IElpidaService _elpidaService;

		public ElpidaController(IElpidaService elpidaService)
		{
			_elpidaService = elpidaService;
		}

		/// <summary>
		///     Get all the Elpida versions with paging.
		/// </summary>
		/// <param name="pageRequest">The page request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Elpida versions requested.</returns>
		/// <response code="200">The returned page of the Elpida versions previews.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<ElpidaDto>> GetPaged(
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return _elpidaService.GetPagedAsync(
				new QueryRequest { PageRequest = pageRequest },
				cancellationToken
			);
		}

		/// <summary>
		///     Get the full details of a single Elpida version.
		/// </summary>
		/// <param name="id">The id of the Elpida version to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The Elpida version details.</returns>
		/// <response code="200">The returned data of the Elpida version.</response>
		/// <response code="404">The Elpida version with this id was not found.</response>
		[HttpGet("{id:long}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public Task<ElpidaDto> GetSingle([FromRoute] long id, CancellationToken cancellationToken)
		{
			return _elpidaService.GetSingleAsync(id, cancellationToken);
		}

		/// <summary>
		///     Search for Elpida versions with the provided criteria.
		/// </summary>
		/// <param name="queryRequest">The query request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Elpida version previews that the search yielded.</returns>
		/// <response code="200">The returned page of the Elpida version previews that the search yielded.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpPost("Search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<ElpidaDto>> Search(
			[FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken
		)
		{
			QueryRequestUtilities.PreprocessQuery(queryRequest);

			return _elpidaService.GetPagedAsync(queryRequest, cancellationToken);
		}
	}
}