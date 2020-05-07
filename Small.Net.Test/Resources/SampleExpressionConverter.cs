using System;
using System.Linq.Expressions;
using Small.Net.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Expressions.Visitor;
using Small.Net.Extensions;

namespace Small.Net.Test.Resources
{
    /// <summary>
    /// Count the expression tree depth 
    /// </summary>
    public class SampleExpressionConverter : ExpressionConverter<int>
    {
        protected override BinaryVisitor<int> CreateBinaryVisitor(BinaryExpression node)
        {
            return new SampleBinaryVisitor(node);
        }

        protected override ConditionalVisitor<int> CreateConditionalVisitor(ConditionalExpression node)
        {
            return new SampleConditionalVisitor(node);
        }

        protected override ConstantVisitor<int> CreateConstantVisitor(ConstantExpression node)
        {
            return new SampleConstantVisitor(node);
        }

        protected override LambdaVisitor<int> CreateLambdaVisitor(LambdaExpression node)
        {
            return new SampleLambdaVisitor(node);
        }

        protected override MethodCallVisitor<int> CreateMethodCallVisitor(MethodCallExpression node)
        {
            return new SampleMethodCallVisitor(node);
        }

        protected override ParameterVisitor<int> CreateParameterVisitor(ParameterExpression node)
        {
            return new SampleParameterVisitor(node);
        }

        protected override UnaryVisitor<int> CreateUnaryVisitor(UnaryExpression node)
        {
            return new SampleUnaryVisitor(node);
        }
    }
}