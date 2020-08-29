using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data
{
	public class AzureBlobsAssetsRepository : IAssetsRepository
	{
		private readonly IAssetsRepositorySettings _assetsRepositorySettings;

		public AzureBlobsAssetsRepository(IAssetsRepositorySettings assetsRepositorySettings)
		{
			_assetsRepositorySettings = assetsRepositorySettings;
		}

		#region IAssetsRepository Members

		public async Task<Uri> CreateAsync(string filename, Stream inputData,
			CancellationToken cancellationToken = default)
		{
			if (inputData == null) throw new ArgumentNullException(nameof(inputData));

			var blobUri = GetBlobUri(filename);
			var client = new BlobClient(blobUri,
				new StorageSharedKeyCredential(_assetsRepositorySettings.AccountName,
					_assetsRepositorySettings.AccountKey));

			await client.UploadAsync(inputData, true, cancellationToken);
			return blobUri;
		}

		public async Task<IEnumerable<AssetInfoModel>> GetAssetsAsync(CancellationToken cancellationToken = default)
		{
			var client = GetContainerClient();
			var returnList = new List<AssetInfoModel>();
			await foreach (var blob in client.GetBlobsAsync())
			{
				returnList.Add(new AssetInfoModel
				{
					Filename = blob.Name,
					Location = GetBlobUri(blob.Name),
					Size = blob.Properties.ContentLength ?? -1,
					Md5 = ByteArrayToString(blob.Properties.ContentHash)
				});
			}

			return returnList;
		}

		#endregion

		private BlobContainerClient GetContainerClient()
		{
			return new BlobContainerClient(GetContainerUri(), new StorageSharedKeyCredential(
				_assetsRepositorySettings.AccountName,
				_assetsRepositorySettings.AccountKey));
		}

		private Uri GetBlobUri(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename))
				throw new ArgumentException("Filename is empty!", nameof(filename));
			return new Uri($"{GetContainerUri()}/{filename}");
		}

		private Uri GetContainerUri()
		{
			return new Uri(
				$"{_assetsRepositorySettings.BlobStorageUri}/{_assetsRepositorySettings.BlobStorageContainer}");
		}

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