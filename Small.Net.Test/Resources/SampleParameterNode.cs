using Small.Net.Expressions;

namespace Small.Net.Test.Resources
{
    public class SampleParameterNode : ParameterNode<int>
    {
        public override int Compute()
        {
            return 1;
        }
    }
}