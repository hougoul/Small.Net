
namespace Small.Net.Expressions.Visitor
{
    public interface IVisitor<TTree, TNodeOutput>
    {
        TNodeOutput Result { get; }

        IVisitor<TTree, TNodeOutput> Visit(TTree expression);
    }
}