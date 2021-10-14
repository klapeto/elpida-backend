using Elpida.Backend.Services.Abstractions;

namespace Elpida.Web.Frontend.Models.Filters
{
	public class StringFilterModel : FilterModel
	{
		public StringFilterModel(string name, string internalName)
			: base(name, internalName)
		{
		}

		public string? Value { get; set; }

		public override bool IsSet => !string.IsNullOrEmpty(Value);

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