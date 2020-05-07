using System.Linq.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Test.Resources
{
    public class SampleBinaryVisitor : BinaryVisitor<int>
    {
        public SampleBinaryVisitor(BinaryExpression node) : base(node)
        {
        }

        public override int Visit(IExpressionConverter<int> converter)
        {
            return 1 + Left(converter) + Right(converter);
        }
    }
}