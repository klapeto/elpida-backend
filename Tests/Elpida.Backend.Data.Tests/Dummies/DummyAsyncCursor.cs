using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elpida.Backend.Data.Tests.Dummies
{
	public class DummyAsyncCursor<TResult> : IAsyncCursor<TResult>
	{
		private readonly IEnumerator<TResult> _internalEnumerator;

		public DummyAsyncCursor(IEnumerable<TResult> @internal)
		{
			Current = @internal;
			_internalEnumerator = Current.GetEnumerator();
		}

		#region IAsyncCursor<TResult> Members

		public void Dispose()
		{
		}

		public bool MoveNext(CancellationToken cancellationToken = new CancellationToken())
		{
			return _internalEnumerator.MoveNext();
		}

		public Task<bool> MoveNextAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			return Task.FromResult(MoveNext(cancellationToken));
		}

		public IEnumerable<TResult> Current { get; }

		#endregion
	}
}