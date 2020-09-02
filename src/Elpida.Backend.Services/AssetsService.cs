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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services
{
	public class AssetsService : IAssetsService
	{
		private readonly IAssetsRepository _assetsRepository;

		public AssetsService(IAssetsRepository assetsRepository)
		{
			_assetsRepository = assetsRepository ?? throw new ArgumentNullException(nameof(assetsRepository));
		}

		#region IAssetsService Members

		public Task<Uri> CreateAsync(string filename, Stream inputData, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(filename))
				throw new ArgumentException("Filename is empty!", nameof(filename));
			if (inputData == null) throw new ArgumentNullException(nameof(inputData));

			return _assetsRepository.CreateAsync(filename, inputData, cancellationToken);
		}

		public async Task<IEnumerable<AssetInfoDto>> GetAssetsAsync(CancellationToken cancellationToken = default)
		{
			return (await _assetsRepository.GetAssetsAsync(cancellationToken))
				.Select(m => m.ToDto())
				.ToArray();
		}

		#endregion
	}
}