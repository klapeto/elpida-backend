using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Web.Frontend.Models
{
	public class ComparisonType
	{
		private static readonly IReadOnlyCollection<ComparisonType> EqualComparisons = new[]
		{
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.Equal], "equals to"),
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.NotEqual], "is not equal to"),
		};

		public static readonly IReadOnlyCollection<ComparisonType> DateComparisons = new[]
		{
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.Greater], "is greater than"),
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.Less], "is less than"),
		};

		public static readonly IReadOnlyCollection<ComparisonType> NumberComparisons = EqualComparisons
			.Concat(DateComparisons)
			.Concat(new[]
		{
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.GreaterEqual], "is greater or equal than"),
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.LessEqual], "is less or equal than"),
		}).ToArray();

		public static readonly IReadOnlyCollection<ComparisonType> StringComparisons = new[]
		{
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.Contains], "contains"),
			new ComparisonType(FilterMaps.ComparisonMap[FilterComparison.NotContain], "does not contain"),
		}.Concat(EqualComparisons).ToArray();

		private ComparisonType(string internalName, string displayName)
		{
			InternalName = internalName;
			DisplayName = displayName;
		}

		public string InternalName { get; }

		public string DisplayName { get; }
	}
}