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
            Initialize();
            var visitor = expression.CreateFromExpression();
            visitor.Visit(this);
            return FinishConversion();
        }

        public abstract void Add(ExpressionLambda lambda);
        public abstract void Add(ExpressionParameter parameter);
        public abstract void Add(ExpressionConstant constant);
        public abstract void Add(ExpressionUnary unary);
        public abstract void Add(ExpressionMethodCall methodCall);
        public abstract void BeginMethodArgument();
        public abstract void EndMethodArgument();
        public abstract void Add(ExpressionBinary binary);
        public abstract void BeginLeftPart();
        public abstract void EndLeftPart();
        public abstract void BeginRightPart();
        public abstract void EndRightPart();
        public abstract void BeginConditional();
        public abstract void EndConditional();
        public abstract void BeginTruePart();
        public abstract void EndTruePart();
        public abstract void BeginFalsePart();
        public abstract void EndFalsePart();

        protected abstract void Initialize();
        protected abstract T FinishConversion();
    }
}