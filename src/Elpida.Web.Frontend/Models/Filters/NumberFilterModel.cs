namespace Elpida.Web.Frontend.Models.Filters
{
	public class NumberFilterModel : ValueFilterModel<long>
	{
		public NumberFilterModel(string name, string internalName)
			: base(name, internalName)
		{
		}

		public override void Reset()
		{
			Value = null;
		}
	}
}