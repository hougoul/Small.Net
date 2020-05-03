namespace Small.Net.Expressions.Visitor
{
    internal interface IExpressionVisitor<TNodeOutput>
    {
        void Visit(IExpressionConverter<TNodeOutput> converter);
        ExpressionNode<TNodeOutput> Node { get; }
    }
}