using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal abstract class ExpressionVisitor<TExpression> : IExpressionVisitor
        where TExpression : Expression
    {
        protected ExpressionVisitor(TExpression node)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public ExpressionType NodeType => Node.NodeType;
        public TExpression Node { get; }

        public abstract void Visit(IExpressionConverter converter);
    }
}