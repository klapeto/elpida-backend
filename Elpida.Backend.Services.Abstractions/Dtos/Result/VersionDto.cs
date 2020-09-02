namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class VersionDto
	{
		public int Major { get; set; }
		public int Minor { get; set; }
		public int Revision { get; set; }
		public int Build { get; set; }
	}
}