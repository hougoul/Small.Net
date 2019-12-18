using System;
using System.Net.Sockets;

namespace Small.Net.Collection
{
    internal abstract class TreeNode<TKey, TValue> : IDisposable
    {
        protected readonly int MaxNodeSize;

        protected TreeNode(int maxNodeSize)
        {
            MaxNodeSize = maxNodeSize;
        }

        public abstract bool IsLeaf { get; }
        public abstract int Count { get; }

        public abstract int ValueCount { get; }

        /// <summary>
        /// Add the item and return the root node
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public abstract TreeNode<TKey, TValue> Add(BpTreeOperation<TKey, TValue> op);

        internal abstract BpTreeResult<TKey, TValue> InternalAdd(BpTreeOperation<TKey, TValue> op);

        protected bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal virtual void ComputeValueCount()
        {
        }
    }
}