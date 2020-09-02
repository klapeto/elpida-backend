namespace Elpida.Backend.Services.Abstractions
{
	public class PageRequest
	{
		public int Next { get; set; }

		public int Count { get; set; } = 10;
		
		public long TotalCount { get; set; }
	}
}