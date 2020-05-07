using System;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class ParameterVisitor<TNodeOutput> : ExpressionVisitor<ParameterExpression, TNodeOutput>
    {
        protected ParameterVisitor(ParameterExpression node) : base(node)
        {
        }
    }
}