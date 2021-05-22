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
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly IBenchmarkService _benchmarkService;

        public BenchmarkController(IBenchmarkService benchmarkService)
        {
            _benchmarkService = benchmarkService;
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
            return Ok(await _benchmarkService.GetPagedPreviewsAsync(new QueryRequest {PageRequest = pageRequest},
                cancellationToken));
        }

        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            return Ok(await _benchmarkService.GetSingleAsync(id, cancellationToken));
        }

        [HttpPost("Search")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] QueryRequest queryRequest,
            CancellationToken cancellationToken)
        {
            QueryRequestUtilities.PreprocessQuery(queryRequest);

            return Ok(await _benchmarkService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
        }
    }
}