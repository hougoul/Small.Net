using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Small.Net.Collection
{
    internal class BpInnerTreeNode<TKey, TValue> : TreeNode<TKey, TValue>
    {
        private const int DefaultMaxNodeSize = 32;
        private readonly List<TKey> _keys = new List<TKey>();
        private readonly List<TreeNode<TKey, TValue>> _children = new List<TreeNode<TKey, TValue>>();
        private int _valueCount;

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
            ComputeValueCount();
        }

        public override bool IsLeaf => false;

        public override int Count => _keys.Count;

        public override int ValueCount => _valueCount;

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
            _valueCount++;
            var result = _children[index].InternalAdd(op);
            if (result.Action == BpResultAction.Nothing)
            {
                return result;
            }

            /* Insert the new node and key */
            _keys.Insert(index, result.Key);
            _children.Insert(index + 1, result.Node);
            if (_keys.Count < MaxNodeSize)
            {
                return BpTreeResult<TKey, TValue>.Empty;
            }

            var splitResult = Split();
            ComputeValueCount();
            splitResult.Node.ComputeValueCount();
            return splitResult;
        }

        internal override TKey GetMaxKey(BpTreeOperation<TKey, TValue> op)
        {
            return _children[_children.Count - 1].GetMaxKey(op);
        }

        public override TreeNode<TKey, TValue> Remove(BpTreeOperation<TKey, TValue> op)
        {
            var result = InternalRemove(op);
            return result.Action == BpResultAction.Nothing ? this : result.Node;
        }

        internal override BpTreeResult<TKey, TValue> InternalRemove(BpTreeOperation<TKey, TValue> op)
        {
            var index = _keys.BinarySearch(op.Key, op.Comparer);
            if (index < 0) index = ~index;
            Debug.Assert(index >= 0);
            Debug.Assert(index < _children.Count);
            var childNode = _children[index];
            var childCount = childNode.Count;
            var result = _children[index].InternalRemove(op);
            if (result.Success)
            {
                _valueCount--;
            }

            if (result.Action == BpResultAction.Nothing)
            {
                return result;
            }

            Debug.Assert(result.Action == BpResultAction.Remove);
            _children.RemoveAt(index);
            if (_children.Count > 0 && _keys.Count == 1)
            {
                _keys.Add(GetMaxKey(op));
            }

            _keys.RemoveAt(index > 0 ? index - 1 : index);
            if (_children.Count == 0)
            {
                return result;
            }

            var success = result.Success;
            result = BpTreeResult<TKey, TValue>.Empty;
            result.Success = success;

            return result;
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

        internal sealed override void ComputeValueCount()
        {
            _valueCount = _children.Sum(n => n.ValueCount);
        }
    }
}