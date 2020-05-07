using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class ExpressionVisitor<TExpression, TNodeOutput> : IExpressionVisitor<TNodeOutput>
        where TExpression : Expression
    {
        protected ExpressionVisitor(TExpression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        protected ExpressionType NodeType => Expression.NodeType;
        protected TExpression Expression { get; }

        public abstract TNodeOutput Visit(IExpressionConverter<TNodeOutput> converter);
    }
}