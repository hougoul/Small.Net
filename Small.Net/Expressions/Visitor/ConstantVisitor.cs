using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    internal class ConstantVisitor<TNodeOutput> : ExpressionVisitor<ConstantExpression, TNodeOutput>
    {
        public ConstantVisitor(ConstantExpression expression) : base(expression)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var constant = Initialise(converter.BeginConstant());
            constant.Type = Expression.Type;
            constant.Value = Expression.Value;
            converter.EndConstant(constant);
        }
    }
}