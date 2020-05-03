using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions
{
    public abstract class ExpressionNode<T>
    {
        public Expression Expression { get; internal set; }

        public abstract T Compute();
    }
}