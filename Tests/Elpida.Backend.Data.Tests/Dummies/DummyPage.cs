using System.Collections.Generic;
using System.Linq;
using Azure;

namespace Elpida.Backend.Data.Tests.Dummies
{
	public class DummyPage<T> : Page<T>
	{
		private readonly IEnumerable<T> _internal;

		public DummyPage(IEnumerable<T> @internal)
		{
			_internal = @internal;
		}

		public override IReadOnlyList<T> Values => _internal.ToArray();
		public override string? ContinuationToken { get; } = null;

		public override Response GetRawResponse()
		{
			return null;
		}
	}
}