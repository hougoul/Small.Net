using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class LambdaVisitor<TNodeOutput> : ExpressionVisitor<LambdaExpression, TNodeOutput>
    {
        protected LambdaVisitor(LambdaExpression expression) : base(expression)
        {
        }

        protected IEnumerable<TNodeOutput> Parameters(IExpressionConverter<TNodeOutput> converter)
        {
            return Expression.Parameters.Select(p => converter.CreateFromExpression(p).Visit(converter));
        }

        protected TNodeOutput Body(IExpressionConverter<TNodeOutput> converter)
        {
            return converter.CreateFromExpression(Expression.Body).Visit(converter);
        }
    }
}