using Elpida.Backend.Services.Abstractions;

namespace Elpida.Web.Frontend.Models.Filters
{
	public abstract class FilterModel
	{
		protected FilterModel(string name, string internalName)
		{
			Name = name;
			InternalName = internalName;
		}

		public string Name { get; }

		public string InternalName { get; }

		public string Comparison { get; set; }

		public abstract bool IsSet { get; }

		public abstract void Reset();

		public abstract FilterInstance CreateFilterInstance();
	}
}