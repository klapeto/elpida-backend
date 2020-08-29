using System;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class AssetInfoModel
	{
		public Uri Location { get; set; }
		
		public long Size { get; set; }
		public string Filename { get; set; }
		
		public string Md5 { get; set; }
	}
}