namespace Elpida.Web.Frontend.Data
{
	public class DownloadLink
	{
		public DownloadLink(string name, string architecture, string fileType, string mainLink, string? checksumLink)
		{
			Name = name;
			Architecture = architecture;
			FileType = fileType;
			MainLink = mainLink;
			ChecksumLink = checksumLink;
		}

		public string Name { get; }

		public string Architecture { get; }

		public string FileType { get; }

		public string MainLink { get; }

		public string? ChecksumLink { get; }
	}
}