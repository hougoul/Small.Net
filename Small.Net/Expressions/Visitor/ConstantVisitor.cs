using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class ConstantVisitor : ExpressionVisitor<ConstantExpression>
    {
        public ConstantVisitor(ConstantExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            var constant = new ExpressionConstant {Type = Node.Type, Value = Node.Value};
            converter.Add(constant);
        }
    }
}