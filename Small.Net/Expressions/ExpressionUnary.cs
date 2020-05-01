using System;
using System.Linq.Expressions;

namespace Small.Net.Expressions
{
    public struct ExpressionUnary
    {
        public ExpressionType UnaryType { get; set; }

        public Type Type { get; set; }
    }
}