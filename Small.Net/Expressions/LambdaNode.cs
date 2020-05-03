using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions
{
    public abstract class LambdaNode<T> : ExpressionNode<T>
    {
        public string Name { get; internal set; }

        public Type ReturnType { get; internal set; }

        public ExpressionNode<T>[] Parameters { get; internal set; }

        public ExpressionNode<T> Body { get; internal set; }
    }
}