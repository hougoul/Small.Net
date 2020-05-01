using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class MethodCallVisitor : ExpressionVisitor<MethodCallExpression>
    {
        public MethodCallVisitor(MethodCallExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            var methodCall = new ExpressionMethodCall()
                {Method = Node.Method, IsStatic = Node.Object == null, ArgumentsCount = Node.Arguments.Count};
            converter.Add(methodCall);
            IExpressionVisitor visitor;
            if (!methodCall.IsStatic)
            {
                visitor = Node.Object.CreateFromExpression();
                visitor.Visit(converter);
            }

            converter.BeginMethodArgument();
            foreach (var argument in Node.Arguments)
            {
                visitor = argument.CreateFromExpression();
                visitor.Visit(converter);
            }

            converter.EndMethodArgument();
        }
    }
}