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