namespace Elpida.Backend.Data
{
	public class ElpidaDatabaseSettings : IElpidaDatabaseSettings
	{
		#region IElpidaDatabaseSettings Members

		public string ElpidaCollectionName { get; set; }
		public string StatisticsCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }

		#endregion
	}
}