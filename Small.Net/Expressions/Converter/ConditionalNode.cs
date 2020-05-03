namespace Small.Net.Expressions.Converter
{
    public abstract class ConditionalNode<T> : ExpressionNode<T>
    {
        public ExpressionNode<T> Test { get; internal set; }

        public ExpressionNode<T> True { get; internal set; }

        public ExpressionNode<T> False { get; internal set; }
    }
}