
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
        protected override void Visit(BinaryExpression expression)
        {
            Result++;
            Compute(expression.Left);
            Compute(expression.Right);
        }

        protected override void Visit(MethodCallExpression expression)
        {
            Result++;
            if (expression.Object != null) Compute(expression.Object);
            expression.Arguments.ForEach(Compute);
        }

        protected override void Visit(UnaryExpression expression)
        {
            Result++;
            Compute(expression.Operand);
        }

        protected override void Visit(LambdaExpression expression)
        {
            Result++;
            expression.Parameters.ForEach(Compute);
            Compute(expression.Body);
        }

        protected override void Visit(ParameterExpression expression)
        {
            Result++;
        }

        protected override void Visit(ConditionalExpression expression)
        {
            Result++;
            Compute(expression.Test);
            Compute(expression.IfTrue);
            Compute(expression.IfFalse);
        }

        protected override void Visit(ConstantExpression expression)
        {
            Result++;
        }
    }
}