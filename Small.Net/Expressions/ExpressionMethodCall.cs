using System.Reflection;

namespace Small.Net.Expressions
{
    public struct ExpressionMethodCall
    {
        public MethodInfo Method { get; set; }

        public bool IsStatic { get; set; }

        public int ArgumentsCount { get; set; }
    }
}