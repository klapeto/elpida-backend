namespace Elpida.Backend.Data
{
	public interface IDocumentRepositorySettings
	{
		string ResultsCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}