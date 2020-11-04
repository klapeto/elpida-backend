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
using System.Linq.Expressions;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend.Services
{
	public class QueryExpressionBuilder
	{
		private readonly IReadOnlyDictionary<string, LambdaExpression> _availableExpressions;

		public QueryExpressionBuilder(IReadOnlyDictionary<string, LambdaExpression> availableExpressions)
		{
			_availableExpressions =
				availableExpressions ?? throw new ArgumentNullException(nameof(availableExpressions));
		}

		public IList<Expression<Func<T, bool>>> Build<T>(IEnumerable<QueryInstance> queryInstances)
		{
			var returnList = new List<Expression<Func<T, bool>>>();

			foreach (var queryInstance in queryInstances)
			{
				if (_availableExpressions.TryGetValue(queryInstance.Name.ToLowerInvariant(), out var expression))
				{
					AddFilter(returnList, queryInstance, expression);
				}
			}

			return returnList;
		}

		public Expression<Func<T, object>> GetOrderBy<T>(QueryRequest queryRequest)
		{
			if (queryRequest.OrderBy == null)
			{
				return null;
			}

			var orderBy = queryRequest.OrderBy.ToLowerInvariant();

			if (_availableExpressions.TryGetValue(orderBy, out var strExpression))
			{
				return Expression.Lambda<Func<T, object>>(Expression.Convert(strExpression.Body, typeof(object)),
					strExpression.Parameters);
			}

			throw new ArgumentException(
				$"OrderBy is not a valid order field. Can be: {string.Join(',', _availableExpressions.Keys)}");
		}

		private void AddFilter<T>(ICollection<Expression<Func<T, bool>>> accumulator,
			QueryInstance instance, LambdaExpression fieldPart)
		{
			if (instance == null)
			{
				return;
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
						Filter.StringComparisons.Contains(instance.Comp) &&
						ComparisonExpressions.ExpressionFactories.TryGetValue(instance.Comp, out var factory))
					{
						middlePart = factory(left, right);
					}
					else
					{
						throw new ArgumentException(
							$"String value filter comparison types can be :[{string.Join(",", Filter.StringComparisons.Select(s => s))}]");
					}
				}
				else
				{
					middlePart =
						ComparisonExpressions.ExpressionFactories
							[Filter.ComparisonMap[Filter.Comparison.Contains]](left, right);
				}
			}
			else
			{
				if (instance.Comp != null)
				{
					if (Filter.NumberComparisons.Contains(instance.Comp) &&
					    ComparisonExpressions.ExpressionFactories.TryGetValue(instance.Comp, out var factory))
					{
						middlePart = factory(left, right);
					}
					else
					{
						throw new ArgumentException(
							$"Numeric value filter comparison types can be :[{string.Join(",", Filter.NumberComparisons.Select(s => s))}]");
					}
				}
				else
				{
					middlePart =
						ComparisonExpressions.ExpressionFactories[Filter.ComparisonMap[Filter.Comparison.Equal]](left,
							right);
				}
			}

			accumulator.Add(Expression.Lambda<Func<T, bool>>(middlePart, parameters));
		}
	}
}