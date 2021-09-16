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
using System.Linq;
using System.Linq.Expressions;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend.Services.Utilities
{
	public static class FiltersTransformer
	{
		public static FilterExpression CreateFilter<TModel, T>(string name, Expression<Func<TModel, T>> expression)
		{
			if (expression.Body.NodeType != ExpressionType.MemberAccess)
			{
				throw new InvalidOperationException("The expression body must be member access of the model");
			}

			return new FilterExpression(name.ToLowerInvariant(), (MemberExpression)expression.Body);
		}

		public static IEnumerable<FilterExpression> ConstructCustomFilters<T, TR>(
			Expression<Func<T, TR>> baseExpression,
			IEnumerable<FilterExpression> filters
		)
		{
			var baseBody = (MemberExpression)baseExpression.Body;
			foreach (var filter in filters)
			{
				yield return new FilterExpression(
					filter.Name,
					GenerateMemberExpression(baseBody, filter.Expression)
				);
			}
		}

		private static MemberExpression GenerateMemberExpression(MemberExpression baseBody, Expression memberExpression)
		{
			var members = new Stack<MemberExpression>();

			while (memberExpression.NodeType == ExpressionType.MemberAccess)
			{
				members.Push((MemberExpression)memberExpression);
				memberExpression = ((MemberExpression)memberExpression).Expression!;
			}

			var returnExpression = baseBody;

			while (members.Any())
			{
				returnExpression = Expression.MakeMemberAccess(returnExpression, members.Pop().Member);
			}

			return returnExpression;
		}
	}
}