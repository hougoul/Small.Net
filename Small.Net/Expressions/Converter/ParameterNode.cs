using System;

namespace Small.Net.Expressions.Converter
{
    public abstract class ParameterNode<T> : ExpressionNode<T>
    {
        public string Name { get; internal set; }

        public Type Type { get; internal set; }

        public bool IsByRef { get; internal set; }
    }
}