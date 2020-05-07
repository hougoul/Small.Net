using System.Linq;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Expressions.Visitor;

namespace Small.Net.Test.Resources
{
    public class SampleLambdaVisitor : LambdaVisitor<int>
    {
        public SampleLambdaVisitor(LambdaExpression expression) : base(expression)
        {
        }

        public override int Visit(IExpressionConverter<int> converter)
        {
            return 1 + Parameters(converter).Sum() + Body(converter);
        }
    }
}