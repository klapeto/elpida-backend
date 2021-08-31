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
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	public class ServiceController<TDto, TPreview, TService> : ControllerBase
		where TDto : FoundationDto
		where TPreview : FoundationDto
		where TService : IService<TDto, TPreview>
	{
		public ServiceController(TService service)
		{
			Service = service;
		}

		protected TService Service { get; }

		/// <summary>
		///     Get all the previews with paging.
		/// </summary>
		/// <param name="pageRequest">The page request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of Benchmarks requested.</returns>
		/// <response code="200">The returned page of the previews.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<TPreview>> GetPagedPreviews(
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return Service.GetPagedPreviewsAsync(
				new QueryRequest(pageRequest, null, null, false),
				cancellationToken
			);
		}

		/// <summary>
		///     Get the full details of a single item.
		/// </summary>
		/// <param name="id">The id of the item to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The Cpu details.</returns>
		/// <response code="200">The returned data of the item.</response>
		/// <response code="404">The item with this id was not found.</response>
		[HttpGet("{id:long}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public Task<TDto> GetSingle([FromRoute] long id, CancellationToken cancellationToken)
		{
			return Service.GetSingleAsync(id, cancellationToken);
		}

		/// <summary>
		///     Search for items with the provided criteria.
		/// </summary>
		/// <param name="queryRequest">The query request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The page of item's previews that the search yielded.</returns>
		/// <response code="200">The returned page of the item's previews that the search yielded.</response>
		/// <response code="400">The request data was invalid.</response>
		[HttpPost("Search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Task<PagedResult<TPreview>> Search(
			[FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken
		)
		{
			return Service.GetPagedPreviewsAsync(
				QueryRequestUtilities.PreProcessQuery(queryRequest),
				cancellationToken
			);
		}
	}
}