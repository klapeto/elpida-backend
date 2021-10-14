using System.Collections.Generic;
using System.Linq;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Models
{
	public class QueryModel
	{
		public QueryModel(IEnumerable<FilterModel> filters)
		{
			Filters = filters.ToArray();
		}

		public FilterModel[] Filters { get; }

		public string? SortBy { get; set; }

		public bool Descending { get; set; }
	}
}