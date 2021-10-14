using Elpida.Backend.Services.Abstractions;

namespace Elpida.Web.Frontend.Models.Filters
{
	public class ValueFilterModel<T> : FilterModel
		where T : struct
	{
		public ValueFilterModel(string name, string internalName)
			: base(name, internalName)
		{
		}

		public T? Value { get; set; }

		public override bool IsSet => Value != null;

		public override void Reset()
		{
			Value = null;
		}

		public override FilterInstance CreateFilterInstance()
		{
			return new (InternalName, Value!, Comparison);
		}

		public override string ToString()
		{
			return $"{InternalName} -> {Comparison} -> {Value}";
		}
	}
}