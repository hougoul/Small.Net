using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    internal class BinaryVisitor<TNodeOutput> : ExpressionVisitor<BinaryExpression, TNodeOutput>
    {
        public BinaryVisitor(BinaryExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var binary = Initialise(converter.BeginBinary(NodeType));

            var visitor = Expression.Left.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            binary.Left = visitor.Node;

            visitor = Expression.Right.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            binary.Right = visitor.Node;

            converter.EndBinary(binary);
        }
    }
}