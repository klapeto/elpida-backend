namespace Elpida.Web.Frontend.Models.Filters
{
	public class OptionModel
	{
		public OptionModel(string? displayName, string? internalName)
		{
			DisplayName = displayName;
			InternalName = internalName;
		}

		public string? DisplayName { get; }

		public string? InternalName { get; }
	}
}