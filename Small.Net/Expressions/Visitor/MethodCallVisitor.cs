using System.Linq;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    internal class MethodCallVisitor<TNodeOutput> : ExpressionVisitor<MethodCallExpression, TNodeOutput>
    {
        public MethodCallVisitor(MethodCallExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var methodCall = Initialise(converter.BeginMethodCall());
            methodCall.IsStatic = Expression.Object == null;
            methodCall.Method = Expression.Method;
            IExpressionVisitor<TNodeOutput> visitor;
            if (!methodCall.IsStatic)
            {
                visitor = Expression.Object.CreateFromExpression<TNodeOutput>();
                visitor.Visit(converter);
                methodCall.Object = visitor.Node;
            }

            methodCall.Arguments = Expression.Arguments.Select(a =>
            {
                visitor = a.CreateFromExpression<TNodeOutput>();
                visitor.Visit(converter);
                return visitor.Node;
            }).ToArray();

            converter.EndMethodCall(methodCall);
        }
    }
}