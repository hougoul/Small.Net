using System.Linq;
using Small.Net.Expressions;

namespace Small.Net.Test.Resources
{
    public class SampleMethodCallNode : MethodCallNode<int>
    {
        public override int Compute()
        {
            return 1 + Object?.Compute() ?? 0 + Arguments.Sum(s => s.Compute());
        }
    }
}