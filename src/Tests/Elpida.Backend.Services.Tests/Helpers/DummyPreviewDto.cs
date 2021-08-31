using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Tests.Helpers
{
	public class DummyPreviewDto : FoundationDto
	{
		public DummyPreviewDto(long id, string data)
			: base(id)
		{
			Data = data;
		}

		public string Data { get; }
	}
}