using System.Linq.Expressions;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Expressions.Converter
{
    public interface IExpressionConverter<TOutput>
    {
        TOutput DefaultValue { get; }
        TOutput Convert(Expression expression);
        IExpressionVisitor<TOutput> CreateFromExpression(Expression expression);
    }
}