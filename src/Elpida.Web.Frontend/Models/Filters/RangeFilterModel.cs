namespace Elpida.Web.Frontend.Models.Filters
{
	public class RangeFilterModel : NumberFilterModel
	{
		public RangeFilterModel(string name, string internalName, long min, long max)
			: base(name, internalName)
		{
			Min = min;
			Max = max;
		}

		public long Max { get; }

		public long Min { get; }
	}
}