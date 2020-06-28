namespace Small.Net.Expressions.Visitor
{
    public interface IVisitor<out TNodeOutput>
    {
        TNodeOutput Result { get; }
    }
}