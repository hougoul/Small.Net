using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class LambdaVisitor : ExpressionVisitor<LambdaExpression>
    {
        public LambdaVisitor(LambdaExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            var lambda = new ExpressionLambda()
                {Name = Node.Name, ReturnType = Node.ReturnType, ParameterCount = Node.Parameters.Count};
            converter.Add(lambda);
            IExpressionVisitor visitor;
            foreach (var parameter in Node.Parameters)
            {
                visitor = parameter.CreateFromExpression();
                visitor.Visit(converter);
            }

            visitor = Node.Body.CreateFromExpression();
            visitor.Visit(converter);
        }
    }
}