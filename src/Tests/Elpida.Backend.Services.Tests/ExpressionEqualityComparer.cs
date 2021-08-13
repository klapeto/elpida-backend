using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Elpida.Backend.Services.Tests
{
	public class ExpressionEqualityComparer<T> : IEqualityComparer<Expression<Func<T, bool>>>
	{
		public bool Equals(Expression<Func<T, bool>> x, Expression<Func<T, bool>> y)
		{
			return x.ToString() == y.ToString();
		}

		public int GetHashCode(Expression<Func<T, bool>> obj)
		{
			return obj.GetHashCode();
		}
	}
}