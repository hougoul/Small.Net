using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class ConditionalVisitor<TNodeOutput> : ExpressionVisitor<ConditionalExpression, TNodeOutput>
    {
        protected ConditionalVisitor(ConditionalExpression node) : base(node)
        {
        }

        protected TNodeOutput Test(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.Test).Visit(converter);
        }

        protected TNodeOutput True(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.IfTrue).Visit(converter);
        }

        protected TNodeOutput False(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.IfFalse).Visit(converter);
        }
    }
}