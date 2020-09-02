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