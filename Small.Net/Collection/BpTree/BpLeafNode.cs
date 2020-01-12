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

        public override int Count => _values.Count;
        public override int ValueCount => _values.Count;

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
            result.Success = true;
            if (_values.Count != MaxNodeSize)
            {
                return result;
            }

            var (split, key) = Split(op);
            if (!split)
            {
                return result;
            }

            /* Split list */
            result.Action = BpResultAction.Add;
            result.Key = key;
            result.Node = _rightLeaf;

            return result;
        }

        internal override TKey GetMaxKey(BpTreeOperation<TKey, TValue> op)
        {
            return op.ExtractKey(_values[_values.Count - 1]);
        }

        public override TreeNode<TKey, TValue> Remove(BpTreeOperation<TKey, TValue> op)
        {
            var result = InternalRemove(op);
            /* in all case if we come here we stay here */
            return this;
        }

        internal override BpTreeResult<TKey, TValue> InternalRemove(BpTreeOperation<TKey, TValue> op)
        {
            var comparer = new BpLeafValueComparer<TKey, TValue>(op.ExtractKey, op.Comparer);
            var index = _values.BinarySearch(op.Value, comparer);
            var result = BpTreeResult<TKey, TValue>.Empty;
            if (index < 0)
            {
                /* we didn't find the object to remove */
                return result;
            }

            if (op.Value.Equals(_values[index]))
            {
                _values.RemoveAt(index);
                result.Success = true;
            }
            else
            {
                /* search item */
                var leftIndex = SearchLeftIndex(index, op.Key, op);
                var rightIndex = SearchRightIndex(index, op.Key, op) - 1;
                var maxCount = ((rightIndex - leftIndex) / 2) + 1;
                var isRemoved = false;
                for (var i = 0; i < maxCount; i++)
                {
                    var leftElement = _values[leftIndex + i];
                    if (op.Value.Equals(leftElement))
                    {
                        _values.RemoveAt(leftIndex + i);
                        result.Success = true;
                        isRemoved = true;
                        break;
                    }

                    var rightElement = _values[rightIndex - i];
                    if (!op.Value.Equals(rightElement))
                    {
                        continue;
                    }

                    _values.RemoveAt(rightIndex - i);
                    result.Success = true;
                    isRemoved = true;
                    break;
                }

                if (!isRemoved)
                {
                    return result;
                }
            }

            if (_values.Count != 0)
            {
                return result;
            }

            switch (_leftLeaf)
            {
                case null when _rightLeaf == null:
                    return result;
                case null:
                    _rightLeaf._leftLeaf = null;
                    result.Action = BpResultAction.Remove;
                    result.Node = this;
                    return result;
            }

            result.Action = BpResultAction.Remove;
            result.Node = this;

            if (_rightLeaf == null)
            {
                _leftLeaf._rightLeaf = null;
                return result;
            }

            _leftLeaf._rightLeaf = _rightLeaf;
            _rightLeaf._leftLeaf = _leftLeaf;
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

        private Tuple<bool, TKey> Split(BpTreeOperation<TKey, TValue> op)
        {
            var (split, index, key) = GetSplitInformation(op);
            if (!split)
            {
                return Tuple.Create(false, key);
            }

            _rightLeaf = new BpLeafNode<TKey, TValue>(MaxNodeSize, this, _rightLeaf);
            _rightLeaf._values.AddRange(_values.GetRange(index, _values.Count - index));
            _values.RemoveRange(index, _values.Count - index);
            return Tuple.Create(true, key);
        }

        private Tuple<bool, int, TKey> GetSplitInformation(BpTreeOperation<TKey, TValue> op)
        {
            var keyFirstElement = op.ExtractKey(_values[0]);
            var keyLastElement = op.ExtractKey(_values[_values.Count - 1]);
            if (keyFirstElement.Equals(keyLastElement))
            {
                return Tuple.Create(false, 0, default(TKey));
            }

            var splitIndex = MaxNodeSize / 2;
            var splitKey = op.ExtractKey(_values[splitIndex - 1]);
            var leftSplit = SearchLeftIndex(splitIndex, splitKey, op);
            var rightSplit = SearchRightIndex(splitIndex, splitKey, op);
            if (leftSplit == 0)
            {
                splitIndex = rightSplit;
            }
            else
            {
                if (leftSplit > (_values.Count - rightSplit))
                {
                    splitIndex = leftSplit;
                    splitKey = op.ExtractKey(_values[splitIndex - 1]);
                }
                else splitIndex = rightSplit;
            }

            if (splitIndex == _values.Count)
            {
                splitIndex--;
            }

            return Tuple.Create(true, splitIndex, splitKey);
        }

        private int SearchLeftIndex(int currentSplitIndex, TKey currentSplitKey, BpTreeOperation<TKey, TValue> op)
        {
            var i = currentSplitIndex - 1;
            while (i >= 0 && currentSplitKey.Equals(op.ExtractKey(_values[i]))) i--;
            return ++i;
        }

        private int SearchRightIndex(int currentSplitIndex, TKey currentSplitKey, BpTreeOperation<TKey, TValue> op)
        {
            var i = currentSplitIndex;
            while (i < _values.Count && currentSplitKey.Equals(op.ExtractKey(_values[i]))) i++;
            return i;
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