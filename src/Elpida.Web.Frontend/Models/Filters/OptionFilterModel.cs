using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Web.Frontend.Models.Filters
{
	public class OptionFilterModel : StringFilterModel
	{
		public OptionFilterModel(
			string name,
			string internalName,
			IEnumerable<OptionModel> options
		)
			: base(name, internalName)
		{
			Comparison = FilterMaps.ComparisonMap[FilterComparison.Contains];
			Options = options.ToArray();
		}

		public OptionModel[] Options { get; }
	}
}