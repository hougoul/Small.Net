using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class MethodCallVisitor<TNodeOutput> : ExpressionVisitor<MethodCallExpression, TNodeOutput>
    {
        protected MethodCallVisitor(MethodCallExpression node) : base(node)
        {
        }

        protected TNodeOutput Object(IExpressionConverter<TNodeOutput> converter)
        {
            return Expression.Object != null
                ? converter.CreateFromExpression(Expression.Object).Visit(converter)
                : converter.DefaultValue;
        }

        protected IEnumerable<TNodeOutput> Arguments(IExpressionConverter<TNodeOutput> converter)
        {
            return Expression.Arguments.Select(a => converter.CreateFromExpression(a).Visit(converter));
        }
    }
}