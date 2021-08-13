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

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public static class Helpers
	{
		public static bool AssertCollectionsAreEqual<T, R>(
			IEnumerable<T> ea,
			IEnumerable<R> eb,
			Func<T, R, bool> assertion
		)
		{
			var a = ea.ToArray();
			var b = eb.ToArray();

			Assert.True(a.Length == b.Length);

			// ReSharper disable once LoopCanBeConvertedToQuery
			for (var i = 0; i < a.Length; i++)
			{
				if (!assertion(a[i], b[i]))
				{
					return false;
				}
			}

			return true;
		}
	}
}