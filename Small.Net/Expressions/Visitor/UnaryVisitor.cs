using System;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class UnaryVisitor<TNodeOutput> : ExpressionVisitor<UnaryExpression, TNodeOutput>
    {
        protected UnaryVisitor(UnaryExpression node) : base(node)
        {
        }

        protected TNodeOutput Operand(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.Operand).Visit(converter);
        }
    }
}