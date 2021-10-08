namespace Elpida.Web.Frontend.Data
{
	public class Filter
	{
		public string Name { get; set; } = string.Empty;

		public string Comparison { get; set; } = "eq";
	}

	public class Filter<T> : Filter
	{
		public T Value { get; set; } = default;
	}
}