using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace Elpida.Backend.Data
{
	public interface IBlobClientFactory
	{
		BlobClient CreateClient(string filename);
		
		BlobContainerClient CreateContainerClient();
	}
}