using System;

namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class AssetInfoDto
	{
		public Uri Location { get; set; }
		
		public long Size { get; set; }
		
		public string Filename { get; set; }
		public string Md5 { get; set; }
	}
}