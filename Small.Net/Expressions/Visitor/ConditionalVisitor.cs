using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    internal class ConditionalVisitor : ExpressionVisitor<ConditionalExpression>
    {
        public ConditionalVisitor(ConditionalExpression node) : base(node)
        {
        }

        public override void Visit(IExpressionConverter converter)
        {
            converter.BeginConditional();
            var visitor = Node.Test.CreateFromExpression();
            visitor.Visit(converter);

            converter.BeginTruePart();
            visitor = Node.IfTrue.CreateFromExpression();
            visitor.Visit(converter);
            converter.EndTruePart();

            converter.BeginFalsePart();
            visitor = Node.IfFalse.CreateFromExpression();
            visitor.Visit(converter);
            converter.EndFalsePart();

            converter.EndConditional();
        }
    }
}