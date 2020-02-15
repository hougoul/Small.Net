using System;
using System.Collections;
using System.Collections.Generic;
using Small.Net.Extensions;

namespace Small.Net.Collection
{
    /// <summary>
    /// B+Tree for class storage and indexing
    ///
    /// TODO Calculate fragmentation and do defragmentation if needed
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public sealed class BpTree<TObject, TProperty> : IDisposable, ICollection<TObject> where TObject : class
    {
        private bool _isDisposed;
        private TreeNode<TProperty, TObject> _rootNode;
        private Func<TObject, TProperty> _accessor;

        public int Count => _rootNode?.ValueCount ?? 0;
        public bool IsReadOnly => false;

        public BpTree(Func<TObject, TProperty> keyAccessor)
        {
            _accessor = keyAccessor ?? throw new ArgumentNullException(nameof(keyAccessor));
        }

        public void Add(TObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_rootNode == null)
            {
                _rootNode = new BpLeafNode<TProperty, TObject>();
            }

            var operation = BpTreeOperation<TProperty, TObject>.Empty;
            operation.Key = _accessor(item);
            operation.Value = item;
            operation.ExtractKey = _accessor;
            _rootNode = _rootNode.Add(operation);
        }

        public bool Remove(TObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_rootNode == null)
            {
                /* Nothing to do */
                return true;
            }

            var operation = BpTreeOperation<TProperty, TObject>.Empty;
            operation.Key = _accessor(item);
            operation.Value = item;
            operation.ExtractKey = _accessor;
            _rootNode = _rootNode.Remove(operation);
            return true;
        }

        public void Clear()
        {
            _rootNode?.Dispose();
            _rootNode = null;
        }

        public bool Contains(TObject item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(TObject[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Clear();
                _isDisposed = true;
            }

            _accessor = null;
        }
    }
}