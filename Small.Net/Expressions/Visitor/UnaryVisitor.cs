using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    internal class UnaryVisitor<TNodeOutput> : ExpressionVisitor<UnaryExpression, TNodeOutput>
    {
        public UnaryVisitor(UnaryExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var unary = Initialise(converter.BeginUnary(NodeType));
            unary.Type = Expression.Type;

            var visitor = Expression.Operand.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            unary.Operand = visitor.Node;

            converter.EndUnary(unary);
        }
    }
}