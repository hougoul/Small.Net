
using Small.Net.Expressions.Visitor;
using System.Linq.Expressions;
using Small.Net.Extensions;

namespace Small.Net.Test.Resources
{
    /// <summary>
    /// Count the expression tree depth 
    /// </summary>
    public class SampleExpressionVisitor : ExpressionVisitor<int>
    {
        public SampleExpressionVisitor()
        {
            Result = 0;
        }
        protected override void Compute(BinaryExpression expression)
        {
            Result++;
            Visit(expression.Left);
            Visit(expression.Right);
        }

        protected override void Compute(MethodCallExpression expression)
        {
            Result++;
            if (expression.Object != null) Visit(expression.Object);
            expression.Arguments.ForEach(a => Visit(a));
        }

        protected override void Compute(UnaryExpression expression)
        {
            Result++;
            Visit(expression.Operand);
        }

        protected override void Compute(LambdaExpression expression)
        {
            Result++;
            expression.Parameters.ForEach(a => Visit(a));
            Visit(expression.Body);
        }

        protected override void Compute(ParameterExpression expression)
        {
            Result++;
        }

        protected override void Compute(ConditionalExpression expression)
        {
            Result++;
            Visit(expression.Test);
            Visit(expression.IfTrue);
            Visit(expression.IfFalse);
        }

        protected override void Compute(ConstantExpression expression)
        {
            Result++;
        }
    }
}