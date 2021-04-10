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
    }
}