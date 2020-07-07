namespace Elpida.Backend.Data.Abstractions.Models
{
	public class StatisticModel
	{
		public string Id { get; set; }
		public string Identifier { get; set; }
		
		public double Value { get; set; }
		
		public ulong Count { get; set; }
	}
}