using Small.Net.Expressions.Visitor;
using System.Linq.Expressions;

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

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Result++;
            return base.VisitBinary(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Result++;
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            Result++;
            return base.VisitUnary(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Result++;
            return base.VisitLambda(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Result++;
            return base.VisitParameter(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Result++;
            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Result++;
            return base.VisitConstant(node);
        }
    }
}