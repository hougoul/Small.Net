using System;

namespace Small.Net.Expressions.Converter
{
    public abstract class LambdaNode<T> : ExpressionNode<T>
    {
        public string Name { get; internal set; }

        public Type ReturnType { get; internal set; }

        public ExpressionNode<T>[] Parameters { get; internal set; }

        public ExpressionNode<T> Body { get; internal set; }
    }
}