using System.Linq.Expressions;

namespace Small.Net.Expressions
{
    public interface IExpressionConverter<TOutput>
    {
        LambdaNode<TOutput> BeginLambda();
        void EndLambda(LambdaNode<TOutput> lambda);
        ParameterNode<TOutput> BeginParameter();
        void EndParameter(ParameterNode<TOutput> parameter);
        ConstantNode<TOutput> BeginConstant();
        void EndConstant(ConstantNode<TOutput> constant);
        UnaryNode<TOutput> BeginUnary(ExpressionType unaryType);
        void EndUnary(UnaryNode<TOutput> unary);

        MethodCallNode<TOutput> BeginMethodCall();

        void EndMethodCall(MethodCallNode<TOutput> methodCall);

        BinaryNode<TOutput> BeginBinary(ExpressionType binaryType);

        void EndBinary(BinaryNode<TOutput> binary);

        ConditionalNode<TOutput> BeginConditional();
        void EndConditional(ConditionalNode<TOutput> condition);
        TOutput Convert(Expression expression);
    }
}