using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class ParameterVisitor<TNodeOutput> : ExpressionVisitor<ParameterExpression, TNodeOutput>
    {
        public ParameterVisitor(ParameterExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var param = Initialise(converter.BeginParameter());
            param.Name = Expression.Name;
            param.Type = Expression.Type;
            param.IsByRef = Expression.IsByRef;
            converter.EndParameter(param);
        }
    }
}