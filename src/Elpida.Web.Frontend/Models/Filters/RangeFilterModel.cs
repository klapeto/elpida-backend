using Elpida.Backend.Services.Abstractions;

namespace Elpida.Web.Frontend.Models.Filters
{
	public class RangeFilterModel : NumberFilterModel
	{
		public RangeFilterModel(
			string name,
			string internalName,
			long min,
			long max,
			FilterComparison comparison,
			string? suffix = null,
			long? value = null
		)
			: base(name, internalName)
		{
			Min = min;
			Max = max;
			Suffix = suffix;
			Value = value;
			Comparison = FilterMaps.ComparisonMap[comparison];
		}

		public long Max { get; }

		public long Min { get; }

		public string? Suffix { get; }
	}
}