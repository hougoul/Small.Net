namespace Small.Net.Expressions.Visitor
{
    internal interface IExpressionVisitor
    {
        void Visit(IExpressionConverter converter);
    }
}