namespace Elpida.Backend.Data
{
	public interface IAssetsRepositorySettings
	{
		public string BlobStorageUri { get; set; }
		
		public string BlobStorageContainer { get; set; }
		
		public string AccountName { get; set; }
		
		public string AccountKey { get; set; }
	}
}