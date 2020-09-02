/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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