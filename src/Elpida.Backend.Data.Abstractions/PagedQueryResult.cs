using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions
{
	public class PagedQueryResult<T>
	{
		public PagedQueryResult(long totalCount, List<T> items)
		{
			TotalCount = totalCount;
			Items = items;
		}

		public long TotalCount { get; }

		public List<T> Items { get; }
	}
}