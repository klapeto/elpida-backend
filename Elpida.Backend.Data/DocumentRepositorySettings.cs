namespace Elpida.Backend.Data
{
	public class DocumentRepositorySettings : IDocumentRepositorySettings
	{
		#region IDocumentRepositorySettings Members

		public string ResultsCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }

		#endregion
	}
}