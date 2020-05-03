using System.Linq;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    internal class LambdaVisitor<TNodeOutput> : ExpressionVisitor<LambdaExpression, TNodeOutput>
    {
        public LambdaVisitor(LambdaExpression expression) : base(expression)
        {
        }

        public override void Visit(IExpressionConverter<TNodeOutput> converter)
        {
            var lambda = Initialise(converter.BeginLambda());

            lambda.Name = Expression.Name;
            lambda.ReturnType = Expression.ReturnType;
            IExpressionVisitor<TNodeOutput> visitor;
            lambda.Parameters = Expression.Parameters.Select(p =>
            {
                visitor = p.CreateFromExpression<TNodeOutput>();
                visitor.Visit(converter);
                return visitor.Node;
            }).ToArray();

            visitor = Expression.Body.CreateFromExpression<TNodeOutput>();
            visitor.Visit(converter);
            lambda.Body = visitor.Node;

            converter.EndLambda(lambda);
        }
    }
}