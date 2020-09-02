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