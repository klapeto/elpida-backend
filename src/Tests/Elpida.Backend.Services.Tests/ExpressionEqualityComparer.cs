// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Elpida.Backend.Services.Tests
{
	internal class ExpressionEqualityComparer<T> : IEqualityComparer<Expression<Func<T, bool>>>
	{
		public bool Equals(Expression<Func<T, bool>>? x, Expression<Func<T, bool>>? y)
		{
			return x?.ToString() == y?.ToString();
		}

		public int GetHashCode(Expression<Func<T, bool>> obj)
		{
			return obj.GetHashCode();
		}
	}
}