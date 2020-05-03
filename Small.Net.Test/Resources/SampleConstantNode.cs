using Small.Net.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Test.Resources
{
    public class SampleConstantNode : ConstantNode<int>
    {
        public override int Compute()
        {
            return 1;
        }
    }
}