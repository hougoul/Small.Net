namespace Small.Net.Collection
{
    internal struct BpTreeResult<TKey, TValue>
    {
        public static BpTreeResult<TKey, TValue> Empty =
            new BpTreeResult<TKey, TValue>(BpResultAction.Nothing, default(TKey), null, false);

        public BpResultAction Action;
        public TKey Key;
        public TreeNode<TKey, TValue> Node;
        public bool Success;

        public BpTreeResult(BpResultAction action, TKey key, TreeNode<TKey, TValue> node, bool success)
        {
            Success = success;
            Action = action;
            Key = key;
            Node = node;
        }
    }
}