using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Small.Net.Expressions.Visitor
{
    public abstract class ExpressionVisitor<TNodeOutput> : ExpressionVisitor, IVisitor<TNodeOutput>
    {
        public TNodeOutput Result { get; protected set; }
    }
}