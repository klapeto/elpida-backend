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
	public class QueryExpressionBuilder
	{
		private readonly IReadOnlyDictionary<string, LambdaExpression> _availableExpressions;

		public QueryExpressionBuilder(IReadOnlyDictionary<string, LambdaExpression> availableExpressions)
		{
			_availableExpressions = availableExpressions;
		}

		public IEnumerable<Expression<Func<T, bool>>> Build<T>(IEnumerable<QueryInstance>? queryInstances)
		{
			if (queryInstances == null)
			{
				yield break;
			}

			foreach (var queryInstance in queryInstances)
			{
				if (!_availableExpressions.TryGetValue(queryInstance.Name.ToLowerInvariant(), out var expression))
				{
					continue;
				}

				var filter = GetFilter<T>(queryInstance, expression);
				if (filter != null)
				{
					yield return filter;
				}
			}
		}

#if false
		public static Expression<Func<T, bool>> BuildOr<T, TR>(Expression<Func<T, TR>> baseExpr, IEnumerable<string> values)
		{
			var baseAccess = baseExpr.Body;

			Expression expr = null;

			foreach (var value in values.ToArray())
			{
				var equal = Expression.Equal(baseAccess, Expression.Constant(value));
				if (expr != null)
				{
					expr = Expression.Or(expr, equal);
				}
				else
				{
					expr = equal;
				}
			}

			return Expression.Lambda<Func<T, bool>>(expr, false, baseExpr.Parameters);
		}
#endif

		public Expression<Func<T, object>>? GetOrderBy<T>(QueryRequest queryRequest)
		{
			if (string.IsNullOrWhiteSpace(queryRequest.OrderBy))
			{
				return null;
			}

			var orderBy = queryRequest.OrderBy.ToLowerInvariant();

			if (_availableExpressions.TryGetValue(orderBy, out var strExpression))
			{
				return Expression.Lambda<Func<T, object>>(
					Expression.Convert(strExpression.Body, typeof(object)),
					strExpression.Parameters
				);
			}

			throw new ArgumentException(
				$"'{queryRequest.OrderBy}' OrderBy is not a valid order field. Can be: {string.Join(',', _availableExpressions.Keys)}"
			);
		}

		private Expression<Func<T, bool>>? GetFilter<T>(QueryInstance? instance, LambdaExpression fieldPart)
		{
			if (instance == null)
			{
				return null;
			}

			Expression right = Expression.Constant(Convert.ChangeType(instance.Value, fieldPart.Body.Type));
			var left = fieldPart.Body;
			var parameters = fieldPart.Parameters;

			Expression middlePart;

			if (instance.Value is string str && !DateTime.TryParse(str, out _))
			{
				if (instance.Comp != null)
				{
					if (
						FilterHelpers.StringComparisons.Contains(instance.Comp)
						&& ComparisonExpressions.ExpressionFactories.TryGetValue(instance.Comp, out var factory))
					{
						middlePart = factory(left, right);
					}
					else
					{
						throw new ArgumentException(
							$"String value filter comparison types can be :[{string.Join(",", FilterHelpers.StringComparisons.Select(s => s))}]"
						);
					}
				}
				else
				{
					var factory =
						ComparisonExpressions.ExpressionFactories[
							FilterHelpers.ComparisonMap[FilterHelpers.Comparison.Contains]];

					middlePart = factory(left, right);
				}
			}
			else
			{
				if (instance.Comp != null)
				{
					if (FilterHelpers.NumberComparisons.Contains(instance.Comp)
					    && ComparisonExpressions.ExpressionFactories.TryGetValue(instance.Comp, out var factory))
					{
						middlePart = factory(left, right);
					}
					else
					{
						throw new ArgumentException(
							$"Numeric value filter comparison types can be :[{string.Join(",", FilterHelpers.NumberComparisons.Select(s => s))}]"
						);
					}
				}
				else
				{
					middlePart =
						ComparisonExpressions.ExpressionFactories[
							FilterHelpers.ComparisonMap[FilterHelpers.Comparison.Equal]](
							left,
							right
						);
				}
			}

			return Expression.Lambda<Func<T, bool>>(middlePart, parameters);
		}
	}
}