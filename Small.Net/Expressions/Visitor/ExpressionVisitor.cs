using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal abstract class ExpressionVisitor<TExpression, TNodeOutput> : IExpressionVisitor<TNodeOutput>
        where TExpression : Expression
    {
        protected ExpressionVisitor(TExpression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        protected ExpressionType NodeType => Expression.NodeType;
        protected TExpression Expression { get; }

        public ExpressionNode<TNodeOutput> Node { get; protected set; }

        public abstract void Visit(IExpressionConverter<TNodeOutput> converter);

        protected T Initialise<T>(T node) where T : ExpressionNode<TNodeOutput>
        {
            node.Expression = Expression;
            Node = node;
            return node;
        }
    }
}