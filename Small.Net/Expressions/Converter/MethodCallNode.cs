using System.Reflection;

namespace Small.Net.Expressions.Converter
{
    public abstract class MethodCallNode<T> : ExpressionNode<T>
    {
        public MethodInfo Method { get; internal set; }

        public bool IsStatic { get; internal set; }

        public ExpressionNode<T> Object { get; internal set; }

        public ExpressionNode<T>[] Arguments { get; internal set; }
    }
}