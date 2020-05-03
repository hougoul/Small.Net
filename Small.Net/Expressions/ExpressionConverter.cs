using System.Linq.Expressions;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Expressions
{
    /// <summary>
    /// Base class to implement for expression converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExpressionConverter<T> : IExpressionConverter<T>
    {
        public T Convert(Expression expression)
        {
            var visitor = expression.CreateFromExpression<T>();
            visitor.Visit(this);
            return visitor.Node.Compute();
        }

        public abstract LambdaNode<T> BeginLambda();
        public abstract void EndLambda(LambdaNode<T> lambda);
        public abstract ParameterNode<T> BeginParameter();
        public abstract void EndParameter(ParameterNode<T> parameter);
        public abstract ConstantNode<T> BeginConstant();
        public abstract void EndConstant(ConstantNode<T> constant);
        public abstract UnaryNode<T> BeginUnary(ExpressionType unaryType);
        public abstract void EndUnary(UnaryNode<T> unary);
        public abstract MethodCallNode<T> BeginMethodCall();
        public abstract void EndMethodCall(MethodCallNode<T> methodCall);
        public abstract BinaryNode<T> BeginBinary(ExpressionType binaryType);
        public abstract void EndBinary(BinaryNode<T> binary);
        public abstract ConditionalNode<T> BeginConditional();
        public abstract void EndConditional(ConditionalNode<T> condition);
    }
}