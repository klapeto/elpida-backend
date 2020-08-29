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
			;
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
					Location = GetBlobUri(client.Uri, blob.Name),
					Size = blob.Properties.ContentLength ?? -1,
					Md5 = ByteArrayToString(blob.Properties.ContentHash)
				});
			}

			return returnList;
		}

		#endregion

		private Uri GetBlobUri(Uri containerUri, string filename)
		{
			if (string.IsNullOrWhiteSpace(filename))
				throw new ArgumentException("Filename is empty!", nameof(filename));

			var uriString = $"{containerUri}/{filename}";

			if (filename.Contains('/'))
				throw new ArgumentException("Filename contains invalid characters", nameof(filename));

			if (!Uri.TryCreate(uriString, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps)
				throw new ArgumentException("Blob Uri is not valid!");

			return uri;
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