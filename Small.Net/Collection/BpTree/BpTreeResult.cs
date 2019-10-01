namespace Small.Net.Collection
{
    internal struct BpTreeResult<TKey, TValue>
    {
        public static BpTreeResult<TKey, TValue> Empty =
            new BpTreeResult<TKey, TValue>(BpResultAction.Nothing, default(TKey), null);

        public BpResultAction Action;
        public TKey Key;
        public TreeNode<TKey, TValue> Node;

        public BpTreeResult(BpResultAction action, TKey key, TreeNode<TKey, TValue> node)
        {
            Action = action;
            Key = key;
            Node = node;
        }
    }
}