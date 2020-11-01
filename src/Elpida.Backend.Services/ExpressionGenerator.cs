using System;
using System.Linq.Expressions;

namespace Elpida.Backend.Services
{
	public class ExpressionGenerator
	{
		public static LambdaExpression GetBaseExpression<T, TReturn>(Expression<Func<T, TReturn>> baseExp)
		{
			// Dirty hack to prevent boxing of values
			return baseExp;
		}
	}
}