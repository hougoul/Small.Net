using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    public abstract class ExpressionVisitor<TNodeOutput> : IVisitor<Expression, TNodeOutput>
    {
        public TNodeOutput Result { get; protected set; }

        public IVisitor<Expression, TNodeOutput> Compute(Expression expression)
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
                    Visit((BinaryExpression)expression);
                    break;
                case ExpressionType.ArrayIndex:
                    if (expression is BinaryExpression binNode) Visit(binNode);
                    else Visit((MethodCallExpression)expression);
                    break;
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
                    Visit((UnaryExpression)expression);
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.Call:
                    Visit((MethodCallExpression)expression);
                    break;
                case ExpressionType.Conditional:
                    Visit((ConditionalExpression)expression);
                    break;
                case ExpressionType.Constant:
                    Visit((ConstantExpression)expression);
                    break;
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
                    Visit((LambdaExpression)expression);
                    break;
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
                    Visit((ParameterExpression)expression);
                    break;
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
                default:
                    throw new NotImplementedException("Expression Unknown");
            }
            return this;
        }

        protected abstract void Visit(BinaryExpression expression);
        protected abstract void Visit(MethodCallExpression expression);
        protected abstract void Visit(UnaryExpression expression);
        protected abstract void Visit(LambdaExpression expression);
        protected abstract void Visit(ParameterExpression expression);
        protected abstract void Visit(ConditionalExpression expression);
        protected abstract void Visit(ConstantExpression expression);
    }
}