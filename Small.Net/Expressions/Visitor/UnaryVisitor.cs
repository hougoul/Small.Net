using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class UnaryVisitor : ExpressionVisitor<UnaryExpression>
    {
        public UnaryVisitor(UnaryExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            var unary = new ExpressionUnary() {Type = Node.Type, UnaryType = NodeType};
            converter.Add(unary);
            var visitor = Node.Operand.CreateFromExpression();
            converter.BeginRightPart();
            visitor.Visit(converter);
            converter.EndRightPart();
        }
    }
}