using System;
using System.Linq.Expressions;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Expressions.Converter
{
    /// <summary>
    /// Base class to implement for expression converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExpressionConverter<T> : IExpressionConverter<T>
    {
        public virtual T DefaultValue { get; } = default(T);

        public T Convert(Expression expression)
        {
            return CreateFromExpression(expression).Visit(this);
        }

        public IExpressionVisitor<T> CreateFromExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Power:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.And:
                case ExpressionType.Or:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.Assign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    return CreateBinaryVisitor((BinaryExpression) expression);
                case ExpressionType.ArrayIndex:
                    if (expression is BinaryExpression binNode) return CreateBinaryVisitor(binNode);
                    return CreateMethodCallVisitor((MethodCallExpression) expression);
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.OnesComplement:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Throw:
                case ExpressionType.Unbox:
                    return CreateUnaryVisitor((UnaryExpression) expression);
                case ExpressionType.Block:
                    break;
                case ExpressionType.Call:
                    return CreateMethodCallVisitor((MethodCallExpression) expression);
                case ExpressionType.Conditional:
                    return CreateConditionalVisitor((ConditionalExpression) expression);
                case ExpressionType.Constant:
                    return CreateConstantVisitor((ConstantExpression) expression);
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.Lambda:
                    return CreateLambdaVisitor((LambdaExpression) expression);
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.Parameter:
                    return CreateParameterVisitor((ParameterExpression) expression);
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.TypeIs:
                    break;
            }

            throw new NotImplementedException("Expression Unknown");
        }

        protected abstract BinaryVisitor<T> CreateBinaryVisitor(BinaryExpression node);

        protected abstract ConditionalVisitor<T> CreateConditionalVisitor(ConditionalExpression node);

        protected abstract ConstantVisitor<T> CreateConstantVisitor(ConstantExpression node);

        protected abstract LambdaVisitor<T> CreateLambdaVisitor(LambdaExpression node);

        protected abstract MethodCallVisitor<T> CreateMethodCallVisitor(MethodCallExpression node);

        protected abstract ParameterVisitor<T> CreateParameterVisitor(ParameterExpression node);

        protected abstract UnaryVisitor<T> CreateUnaryVisitor(UnaryExpression node);
    }
}