using System.Collections.Generic;
using System.Threading;
using Azure;

namespace Elpida.Backend.Data.Tests.Dummies
{
	public class DummyAsyncPageable<T> : AsyncPageable<T>
	{
		private readonly IEnumerable<T> _internal;

		public DummyAsyncPageable(IEnumerable<T> @internal)
		{
			_internal = @internal;
		}


		public override IAsyncEnumerator<T> GetAsyncEnumerator(
			CancellationToken cancellationToken = new CancellationToken())
		{
			return new DummyAsyncEnumerator<T>(_internal.GetEnumerator());
		}

		public override IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
		{
			return new DummyAsyncPageable<Page<T>>(new Page<T>[] {new DummyPage<T>(_internal)});
		}
	}
}