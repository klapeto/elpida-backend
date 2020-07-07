namespace Elpida.Backend.Data.Abstractions.Models
{
	public class TaskResultModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double Value { get; set; }
		public string Suffix { get; set; }
		public int Type { get; set; }
		public double Time { get; set; }
		public ulong InputSize { get; set; }
	}
}