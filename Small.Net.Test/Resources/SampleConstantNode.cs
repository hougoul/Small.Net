using System.Linq.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Test.Resources
{
    public class SampleConstantVisitor : ConstantVisitor<int>
    {
        public SampleConstantVisitor(ConstantExpression expression) : base(expression)
        {
        }

        public override int Visit(IExpressionConverter<int> converter)
        {
            return 1;
        }
    }
}