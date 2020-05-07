using System;
using System.Linq.Expressions;
using Small.Net.Expressions.Converter;

namespace Small.Net.Expressions.Visitor
{
    public abstract class ConstantVisitor<TNodeOutput> : ExpressionVisitor<ConstantExpression, TNodeOutput>
    {
        protected ConstantVisitor(ConstantExpression expression) : base(expression)
        {
        }
    }
}