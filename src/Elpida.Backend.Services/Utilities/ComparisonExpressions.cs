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
using System.Reflection;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend.Services.Utilities
{
	public static class ComparisonExpressions
	{
		private static readonly MethodInfo Contains =
			typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;

		public static IReadOnlyDictionary<string, Func<Expression, Expression, Expression>>
			ExpressionFactories { get; } = new Dictionary<string, Func<Expression, Expression, Expression>>
		{
			[FilterMaps.ComparisonMap[FilterComparison.Contains]] =
				(left, right) => Expression.Call(left, Contains, right),

			[FilterMaps.ComparisonMap[FilterComparison.NotContain]] =
				(left, right) => Expression.Not(Expression.Call(left, Contains, right)),
			[FilterMaps.ComparisonMap[FilterComparison.Equal]] = Expression.Equal,
			[FilterMaps.ComparisonMap[FilterComparison.NotEqual]] = Expression.NotEqual,
			[FilterMaps.ComparisonMap[FilterComparison.Greater]] = Expression.GreaterThan,
			[FilterMaps.ComparisonMap[FilterComparison.GreaterEqual]] = Expression.GreaterThanOrEqual,
			[FilterMaps.ComparisonMap[FilterComparison.Less]] = Expression.LessThan,
			[FilterMaps.ComparisonMap[FilterComparison.LessEqual]] = Expression.LessThanOrEqual,
		};
	}
}