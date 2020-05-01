using System;

namespace Small.Net.Expressions
{
    public struct ExpressionParameter
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public bool IsByRef { get; set; }
    }
}