using System.Linq.Expressions;

namespace Small.Net.Expressions
{
    public interface IExpressionConverter
    {
        void Add(ExpressionLambda lambda);
        void Add(ExpressionParameter parameter);

        void Add(ExpressionConstant constant);

        void Add(ExpressionUnary unary);

        /* Method Call */
        void Add(ExpressionMethodCall methodCall);
        void BeginMethodArgument();
        void EndMethodArgument();

        /* Binary section */
        void Add(ExpressionBinary binary);
        void BeginLeftPart();
        void EndLeftPart();
        void BeginRightPart();
        void EndRightPart();

        /*Conditional Section*/
        void BeginConditional();
        void EndConditional();
        void BeginTruePart();
        void EndTruePart();
        void BeginFalsePart();
        void EndFalsePart();
    }

    public interface IExpressionConverter<out TOutput> : IExpressionConverter
    {
        TOutput Convert(Expression expression);
    }
}