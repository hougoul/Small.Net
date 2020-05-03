using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    internal class ConditionalVisitor<TNodeOutput> : ExpressionVisitor<ConditionalExpression, TNodeOutput>
    {
        public ConditionalVisitor(ConditionalExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var condition = Initialise(converter.BeginConditional());

            var visitor = Expression.Test.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            condition.Test = visitor.Node;

            visitor = Expression.IfTrue.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            condition.True = visitor.Node;

            visitor = Expression.IfFalse.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            condition.False = visitor.Node;

            converter.EndConditional(condition);
        }
    }
}