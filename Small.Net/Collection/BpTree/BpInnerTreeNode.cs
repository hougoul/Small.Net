using System.Collections.Generic;
using System.Diagnostics;

namespace Small.Net.Collection
{
    internal class BpInnerTreeNode<TKey, TValue> : TreeNode<TKey, TValue>
    {
        private const int DefaultMaxNodeSize = 32;
        private readonly List<TKey> _keys = new List<TKey>();
        private readonly List<TreeNode<TKey, TValue>> _children = new List<TreeNode<TKey, TValue>>();

        private BpInnerTreeNode(int maxNodeSize = DefaultMaxNodeSize) : base(maxNodeSize)
        {
        }

        internal BpInnerTreeNode(
            TKey initialKey,
            TreeNode<TKey, TValue> initialLeftNode,
            TreeNode<TKey, TValue> initialRightNode,
            int maxNodeSize = DefaultMaxNodeSize)
            : base(maxNodeSize)
        {
            _keys.Add(initialKey);
            _children.Add(initialLeftNode);
            _children.Add(initialRightNode);
        }

        public override bool IsLeaf => false;

        public override int Count => _keys.Count;

        public override TreeNode<TKey, TValue> Add(BpTreeOperation<TKey, TValue> op)
        {
            var result = InternalAdd(op);
            return result.Action == BpResultAction.Nothing
                ? this
                : new BpInnerTreeNode<TKey, TValue>(result.Key, this, result.Node, MaxNodeSize);
        }

        internal override BpTreeResult<TKey, TValue> InternalAdd(BpTreeOperation<TKey, TValue> op)
        {
            var index = _keys.BinarySearch(op.Key, op.Comparer);
            if (index < 0) index = ~index;
            Debug.Assert(index >= 0);
            Debug.Assert(index < _children.Count);
            var result = _children[index].InternalAdd(op);
            if (result.Action == BpResultAction.Nothing)
            {
                return result;
            }

            /* Insert the new node and key */
            _keys.Insert(index, result.Key);
            _children.Insert(index + 1, result.Node);
            return _keys.Count < MaxNodeSize ? BpTreeResult<TKey, TValue>.Empty : Split();
        }

        private BpTreeResult<TKey, TValue> Split()
        {
            var result = BpTreeResult<TKey, TValue>.Empty;
            result.Action = BpResultAction.Add;
            var divAt = MaxNodeSize / 2;
            result.Key = _keys[divAt];
            var removeKeyIndex = divAt;
            divAt++;
            var node = new BpInnerTreeNode<TKey, TValue>(MaxNodeSize);
            var count = _children.Count - divAt;
            node._children.AddRange(_children.GetRange(divAt, count));
            _children.RemoveRange(divAt, count);
            count = _keys.Count - divAt;
            node._keys.AddRange(_keys.GetRange(divAt, count));
            _keys.RemoveRange(removeKeyIndex, _keys.Count - removeKeyIndex);
            result.Node = node;
            return result;
        }
    }
}