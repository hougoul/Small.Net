using System.Linq;
using Small.Net.Expressions;

namespace Small.Net.Test.Resources
{
    public class SampleLambdaNode : LambdaNode<int>
    {
        public override int Compute()
        {
            return 1 + Parameters.Sum(p => p.Compute()) + Body.Compute();
        }
    }
}