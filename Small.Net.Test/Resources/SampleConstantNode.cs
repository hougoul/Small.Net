using Small.Net.Expressions;

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