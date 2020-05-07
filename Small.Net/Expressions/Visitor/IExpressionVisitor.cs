using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public interface IExpressionVisitor<TNodeOutput>
    {
        TNodeOutput Visit(IExpressionConverter<TNodeOutput> converter);
    }
}