using System;

namespace Small.Net.Expressions
{
    public struct ExpressionLambda
    {
        public string Name { get; set; }

        public Type ReturnType { get; set; }

        public int ParameterCount { get; set; }
    }
}