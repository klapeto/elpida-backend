namespace Elpida.Web.Frontend.Models
{
	public class CategoryModel
	{
		public CategoryModel(string name, string link, string iconLink)
		{
			Name = name;
			Link = link;
			IconLink = iconLink;
		}

		public string Name { get; }

		public string Link { get; }

		public string IconLink { get; }
	}
}