using System;

namespace Small.Net.Expressions.Converter
{
    public abstract class UnaryNode<T> : ExpressionNode<T>
    {
        public Type Type { get; internal set; }

        public ExpressionNode<T> Operand { get; internal set; }
    }
}