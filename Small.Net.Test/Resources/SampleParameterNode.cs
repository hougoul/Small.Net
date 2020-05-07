using System.Linq.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Test.Resources
{
    public class SampleParameterVisitor : ParameterVisitor<int>
    {
        public SampleParameterVisitor(ParameterExpression node) : base(node)
        {
        }

        public override int Visit(IExpressionConverter<int> converter)
        {
            return 1;
        }
    }
}