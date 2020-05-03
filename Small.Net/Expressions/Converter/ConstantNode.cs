using System;

namespace Small.Net.Expressions.Converter
{
    public abstract class ConstantNode<T> : ExpressionNode<T>
    {
        public Type Type { get; internal set; }

        public object Value { get; internal set; }
    }
}