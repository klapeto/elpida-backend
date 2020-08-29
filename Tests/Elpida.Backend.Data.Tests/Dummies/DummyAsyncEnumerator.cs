using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elpida.Backend.Data.Tests.Dummies
{
	public class DummyAsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _internal;

		public DummyAsyncEnumerator(IEnumerator<T> @internal)
		{
			_internal = @internal;
		}

		#region IAsyncEnumerator<T> Members

		public ValueTask DisposeAsync()
		{
			return new ValueTask();
		}

		public ValueTask<bool> MoveNextAsync()
		{
			return new ValueTask<bool>(_internal.MoveNext());
		}

		public T Current => _internal.Current;

		#endregion
	}
}