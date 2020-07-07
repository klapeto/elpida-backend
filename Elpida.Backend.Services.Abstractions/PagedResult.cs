using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions
{
	public class PagedResult<T>
	{
		public PagedResult()
		{
		}

		public PagedResult(IList<T> list, PageRequest pageRequest)
		{
			Count = list.Count;
			List = list;
			TotalCount = pageRequest.TotalCount;
			NextPage = list.Count == pageRequest.Count
				? new PageRequest {Count = pageRequest.Count, Next = pageRequest.Next + list.Count}
				: null;
		}

		public IList<T> List { get; set; }

		public int Count { get; set; }
		
		public long TotalCount { get; set; }

		public PageRequest NextPage { get; set; }
	}
}