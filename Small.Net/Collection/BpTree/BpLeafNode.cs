using System;
using System.Collections.Generic;

namespace Small.Net.Collection
{
    internal class BpLeafNode<TKey, TValue> : TreeNode<TKey, TValue>
    {
        private const int DefaultMaxNodeSize = 64;

        private readonly List<TValue> _values = new List<TValue>();
        private BpLeafNode<TKey, TValue> _leftLeaf;
        private BpLeafNode<TKey, TValue> _rightLeaf;

        public BpLeafNode(int maxNodeSize = DefaultMaxNodeSize,
            BpLeafNode<TKey, TValue> leftLeaf = null,
            BpLeafNode<TKey, TValue> rightLeaf = null)
            : base(maxNodeSize)
        {
            if (maxNodeSize < 3)
                throw new ArgumentOutOfRangeException(
                    nameof(maxNodeSize),
                    "Minimum is 3");
            _leftLeaf = leftLeaf;
            _rightLeaf = rightLeaf;
        }

        public override bool IsLeaf => true;

        public override TreeNode<TKey, TValue> Add(BpTreeOperation<TKey, TValue> op)
        {
            var result = InternalAdd(op);
            /* If I'm here that mean I'm the root node  */
            if (result.Action == BpResultAction.Nothing)
            {
                return this;
            }

            /* Node was split --> create a BpInnerTreeNode */
            return new BpInnerTreeNode<TKey, TValue>(result.Key, this, result.Node);
            ;
        }

        /// <summary>
        /// Do the internal add
        /// </summary>
        /// <param name="op"></param>
        /// <returns>Return Action = nothing, if there is no split</returns>
        internal override BpTreeResult<TKey, TValue> InternalAdd(BpTreeOperation<TKey, TValue> op)
        {
            var comparer = new BpLeafValueComparer<TKey, TValue>(op.ExtractKey, op.Comparer);
            var index = _values.BinarySearch(op.Value, comparer);
            if (index < 0) index = ~index;
            _values.Insert(index, op.Value);
            var result = BpTreeResult<TKey, TValue>.Empty;
            if (_values.Count != MaxNodeSize)
            {
                return result;
            }

            /* Split list */
            result.Action = BpResultAction.Add;
            result.Key = Split(op);
            result.Node = _rightLeaf;

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            _values?.Clear();
            _leftLeaf = null;
            _rightLeaf = null;

            base.Dispose(disposing);
        }

        private TKey Split(BpTreeOperation<TKey, TValue> op)
        {
            var divAt = MaxNodeSize / 2;
            var mid = _values[divAt];
            var splitKey = op.ExtractKey(mid);
            _rightLeaf = new BpLeafNode<TKey, TValue>(MaxNodeSize, this, _rightLeaf);
            _rightLeaf._values.AddRange(_values.GetRange(divAt, _values.Count - divAt));
            _values.RemoveRange(divAt, _values.Count - divAt);
            return splitKey;
        }

        private int MinNumberOfChildren()
        {
            return MaxNodeSize / 2;
        }

        private int MaxNumberOfChildren()
        {
            return MaxNodeSize;
        }
    }
}