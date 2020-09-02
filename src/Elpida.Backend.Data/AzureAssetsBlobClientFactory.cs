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
using Azure.Storage;
using Azure.Storage.Blobs;

namespace Elpida.Backend.Data
{
	public class AzureAssetsBlobClientFactory : IBlobClientFactory
	{
		private readonly IAssetsRepositorySettings _assetsRepositorySettings;
		private readonly Uri _containerUri;

		public AzureAssetsBlobClientFactory(IAssetsRepositorySettings assetsRepositorySettings)
		{
			_assetsRepositorySettings = assetsRepositorySettings ??
			                            throw new ArgumentNullException(nameof(assetsRepositorySettings));
			if (string.IsNullOrWhiteSpace(_assetsRepositorySettings.BlobStorageUri))
				throw new ArgumentException("Blob Uri is empty!", nameof(_assetsRepositorySettings.BlobStorageUri));
			if (string.IsNullOrWhiteSpace(_assetsRepositorySettings.BlobStorageContainer))
				throw new ArgumentException("Blob container name is empty!",
					nameof(_assetsRepositorySettings.BlobStorageContainer));

			if (!Uri.TryCreate(
				$"{_assetsRepositorySettings.BlobStorageUri}/{_assetsRepositorySettings.BlobStorageContainer}",
				UriKind.Absolute, out _containerUri) || _containerUri.Scheme != Uri.UriSchemeHttps)
				throw new ArgumentException("Invalid Uri for blob container");
		}

		#region IBlobClientFactory Members

		public BlobClient CreateClient(string filename)
		{
			return new BlobClient(new Uri($"{_containerUri}/{filename}"),
				new StorageSharedKeyCredential(_assetsRepositorySettings.AccountName,
					_assetsRepositorySettings.AccountKey));
		}

		public BlobContainerClient CreateContainerClient()
		{
			return new BlobContainerClient(_containerUri,
				new StorageSharedKeyCredential(_assetsRepositorySettings.AccountName,
					_assetsRepositorySettings.AccountKey));
		}

		#endregion
	}
}