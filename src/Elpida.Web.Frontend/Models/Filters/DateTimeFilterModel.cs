using System;

namespace Elpida.Web.Frontend.Models.Filters
{
	public class DateTimeFilterModel : ValueFilterModel<DateTime>
	{
		public DateTimeFilterModel(string name, string internalName)
			: base(name, internalName)
		{
		}
	}
}