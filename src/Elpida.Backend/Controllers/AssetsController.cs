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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class AssetsController : Controller
	{
		private const long FileLimit = 300 * 1024 * 1024; // 300Mib Should be enough for now

		private readonly IAssetsService _assetsService;

		public AssetsController(IAssetsService assetsService)
		{
			_assetsService = assetsService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
			return Ok(await _assetsService.GetAssetsAsync(cancellationToken));
		}

		[HttpPost]
		[ApiKeyAuthentication(KeyName = "AssetsKey")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[RequestSizeLimit(FileLimit)]
		[RequestFormLimits(MultipartBodyLengthLimit = FileLimit)]
		public async Task<IActionResult> Post(IFormFile file, CancellationToken cancellationToken)
		{
			return Created(await _assetsService.CreateAsync(file.FileName, file.OpenReadStream(), cancellationToken),
				null);
		}
	}
}