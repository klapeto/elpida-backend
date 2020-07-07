namespace Elpida.Backend.Data
{
	public interface IElpidaDatabaseSettings
	{
		string ElpidaCollectionName { get; set; }
		
		string StatisticsCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}