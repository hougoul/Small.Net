using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions
{
    public abstract class BinaryNode<T> : ExpressionNode<T>
    {
        public ExpressionNode<T> Left { get; internal set; }

        public ExpressionNode<T> Right { get; internal set; }
    }
}