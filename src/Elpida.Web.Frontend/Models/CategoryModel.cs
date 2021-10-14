namespace Elpida.Web.Frontend.Models
{
	public class CategoryModel
	{
		public CategoryModel(string name, string link, string iconName)
		{
			Name = name;
			Link = link;
			IconName = iconName;
		}

		public string Name { get; }

		public string Link { get; }

		public string IconName { get; }
	}
}