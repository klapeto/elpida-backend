namespace Elpida.Backend.Data.Abstractions.Models.Task
{
	public class ResultSpecificationModel
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Unit { get; set; } = string.Empty;
		public int Aggregation { get; set; }
		public int Type { get; set; }
	}
}