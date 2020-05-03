using Small.Net.Expressions;

namespace Small.Net.Test.Resources
{
    public class SampleConditionalNode : ConditionalNode<int>
    {
        public override int Compute()
        {
            return 1 + Test.Compute() + True.Compute() + False.Compute();
        }
    }
}