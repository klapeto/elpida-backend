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
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend.Services
{
	public static class ComparisonExpressions
	{
		private static readonly MethodInfo RegexIsMatch =
			typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string), typeof(string)});

		public static IReadOnlyDictionary<string, Func<Expression, Expression, Expression>>
			ExpressionFactories { get; } = new Dictionary<string, Func<Expression, Expression, Expression>>
		{
			[Filter.ComparisonMap[Filter.Comparison.Contains]] =
				(left, right) => Expression.Call(RegexIsMatch, left, right),
			[Filter.ComparisonMap[Filter.Comparison.Equal]] = Expression.Equal,
			[Filter.ComparisonMap[Filter.Comparison.Greater]] = Expression.GreaterThan,
			[Filter.ComparisonMap[Filter.Comparison.GreaterEqual]] = Expression.GreaterThanOrEqual,
			[Filter.ComparisonMap[Filter.Comparison.Less]] = Expression.LessThan,
			[Filter.ComparisonMap[Filter.Comparison.LessEqual]] = Expression.LessThanOrEqual
		};
	}
}