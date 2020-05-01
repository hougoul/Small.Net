using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class BinaryVisitor : ExpressionVisitor<BinaryExpression>
    {
        public BinaryVisitor(BinaryExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            var binary = new ExpressionBinary()
            {
                BinaryType = NodeType
            };
            converter.Add(binary);

            var visitor = Node.Left.CreateFromExpression();
            converter.BeginLeftPart();
            visitor.Visit(converter);
            converter.EndLeftPart();

            visitor = Node.Right.CreateFromExpression();
            converter.BeginRightPart();
            visitor.Visit(converter);
            converter.EndRightPart();
        }
    }
}