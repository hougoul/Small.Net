using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class BinaryVisitor<TNodeOutput> : ExpressionVisitor<BinaryExpression, TNodeOutput>
    {
        protected BinaryVisitor(BinaryExpression node) : base(node)
        {
        }

        protected TNodeOutput Left(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.Left).Visit(converter);
        }

        protected TNodeOutput Right(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.Right).Visit(converter);
        }
    }
}