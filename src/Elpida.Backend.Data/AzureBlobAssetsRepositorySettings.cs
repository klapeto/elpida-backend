namespace Elpida.Backend.Data
{
	public class AzureBlobAssetsRepositorySettings : IAssetsRepositorySettings
	{
		#region IAssetsRepositorySettings Members

		public string BlobStorageUri { get; set; }
		public string BlobStorageContainer { get; set; }
		public string AccountName { get; set; }
		public string AccountKey { get; set; }

		#endregion
	}
}