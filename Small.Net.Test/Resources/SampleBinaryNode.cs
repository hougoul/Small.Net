using Small.Net.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Test.Resources
{
    public class SampleBinaryNode : BinaryNode<int>
    {
        public override int Compute()
        {
            return 1 + Left.Compute() + Right.Compute();
        }
    }
}