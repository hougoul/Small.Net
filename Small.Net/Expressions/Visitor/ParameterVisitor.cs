using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class ParameterVisitor : ExpressionVisitor<ParameterExpression>
    {
        public ParameterVisitor(ParameterExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            var param = new ExpressionParameter {Name = Node.Name, Type = Node.Type, IsByRef = Node.IsByRef};
            converter.Add(param);
        }
    }
}