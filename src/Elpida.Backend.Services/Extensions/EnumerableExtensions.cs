using System;
using System.Collections.Generic;

namespace Elpida.Backend.Services.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var existingItems = new HashSet<TKey>();
			foreach (var element in source)
			{
				if (existingItems.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}
	}
}