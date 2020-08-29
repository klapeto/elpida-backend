namespace Elpida.Backend.Data
{
	public class DocumentRepositorySettings : IDocumentRepositorySettings
	{
		#region ResultsRepositorySettings Members

		public string ResultsCollectionName { get; set; }
		
		public string StatisticsCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }

		#endregion
	}
}