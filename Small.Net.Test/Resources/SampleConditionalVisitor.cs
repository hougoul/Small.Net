using System.Linq.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Test.Resources
{
    public class SampleConditionalVisitor : ConditionalVisitor<int>
    {
        public SampleConditionalVisitor(ConditionalExpression node) : base(node)
        {
        }

        public override int Visit(IExpressionConverter<int> converter)
        {
            return 1 + Test(converter) + True(converter) + False(converter);
        }
    }
}