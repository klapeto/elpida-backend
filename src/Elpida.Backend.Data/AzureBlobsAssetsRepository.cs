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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data
{
	public class AzureBlobsAssetsRepository : IAssetsRepository
	{
		private readonly IBlobClientFactory _blobClientFactory;

		public AzureBlobsAssetsRepository(IBlobClientFactory blobClientFactory)
		{
			_blobClientFactory = blobClientFactory ?? throw new ArgumentNullException(nameof(blobClientFactory));
		}

		#region IAssetsRepository Members

		public async Task<Uri> CreateAsync(string filename, Stream inputData,
			CancellationToken cancellationToken = default)
		{
			if (inputData == null) throw new ArgumentNullException(nameof(inputData));
			if (string.IsNullOrWhiteSpace(filename) || filename.Contains('/'))
				throw new ArgumentException("Invalid filename", nameof(filename));

			var client = _blobClientFactory.CreateClient(filename);

			await client.UploadAsync(inputData, true, cancellationToken);
			return client.Uri;
		}

		public async Task<IEnumerable<AssetInfoModel>> GetAssetsAsync(CancellationToken cancellationToken = default)
		{
			var client = _blobClientFactory.CreateContainerClient();
			var returnList = new List<AssetInfoModel>();
			await foreach (var blob in client.GetBlobsAsync())
			{
				returnList.Add(new AssetInfoModel
				{
					Filename = blob.Name,
					Location = new Uri($"{client.Uri}/{blob.Name}"),
					Size = blob.Properties.ContentLength ?? -1,
					Md5 = ByteArrayToString(blob.Properties.ContentHash)
				});
			}

			return returnList;
		}

		#endregion

		private static string ByteArrayToString(IReadOnlyCollection<byte> bytes)
		{
			var strBuilder = new StringBuilder(bytes.Count);
			foreach (var b in bytes)
			{
				strBuilder.Append(b.ToString("X"));
			}

			return strBuilder.ToString();
		}
	}
}