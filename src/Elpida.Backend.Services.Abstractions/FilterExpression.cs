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
using System.Linq.Expressions;

namespace Elpida.Backend.Services.Abstractions
{
	public class FilterExpression
	{
		public FilterExpression(string name, MemberExpression expression)
		{
			Name = name;
			Expression = expression;
		}

		public string Name { get; }

		public MemberExpression Expression { get; }

		public override bool Equals(object? obj)
		{
			if (obj is FilterExpression expression)
			{
				return expression.Name == Name;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name);
		}

		protected bool Equals(FilterExpression other)
		{
			return Name == other.Name;
		}
	}
}