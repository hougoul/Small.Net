using System;

namespace Small.Net.Expressions
{
    public struct ExpressionConstant
    {
        public Type Type { get; set; }

        public object Value { get; set; }
    }
}