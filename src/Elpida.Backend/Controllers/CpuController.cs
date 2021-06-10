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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class CpuController : ControllerBase
	{
		private readonly ICpuService _cpuService;
		private readonly ITopologyService _topologyService;

		public CpuController(ICpuService cpuService, ITopologyService topologyService)
		{
			_cpuService = cpuService;
			_topologyService = topologyService;
		}

		[HttpGet]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetPaged(
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return Ok(
				await _cpuService.GetPagedPreviewsAsync(
					new QueryRequest { PageRequest = pageRequest },
					cancellationToken
				)
			);
		}

		[HttpGet("{id:long}", Name = nameof(GetSingleCpu))]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSingleCpu([FromRoute] long id, CancellationToken cancellationToken)
		{
			return Ok(await _cpuService.GetSingleAsync(id, cancellationToken));
		}

		[HttpGet("{id:long}/Topologies")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTopologies(
			[FromRoute] long id,
			[FromQuery] PageRequest pageRequest,
			CancellationToken cancellationToken
		)
		{
			return Ok(
				await _topologyService.GetPagedAsync(
					new QueryRequest
					{
						Filters = new[] { GetQueryInstanceForId(id) },
						PageRequest = pageRequest,
					},
					cancellationToken
				)
			);
		}

		[HttpPost("Search")]
		[Produces("application/json")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Search(
			[FromBody] QueryRequest queryRequest,
			CancellationToken cancellationToken
		)
		{
			QueryRequestUtilities.PreprocessQuery(queryRequest);

			return Ok(await _cpuService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
		}

		private static QueryInstance GetQueryInstanceForId(long cpuId)
		{
			return new ()
			{
				Comp = FilterHelpers.ComparisonMap[FilterHelpers.Comparison.Equal],
				Name = "cpuId",
				Value = cpuId,
			};
		}
	}
}