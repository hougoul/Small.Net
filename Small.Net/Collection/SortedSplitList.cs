using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/*
 * Code Base on SortedSplitList, by Aurelien BOUDOUX.
 * 
 */
namespace Small.Net.Collection
{
    public class SortedSplitList<T> : IEnumerable<T>
    {
        private static readonly CompareByBeginIndex<T> IndexComparer = new CompareByBeginIndex<T>();
        private static readonly ReaderWriterLockSlim ReaderWriterLock = new ReaderWriterLockSlim();

        private readonly int _deepness;
        private readonly IComparer<T> _defaultComparer;
        private readonly CompareByFirstItem<T> _verticalComparer;
        private readonly List<SortedSplitNode<T>> _verticalIndex = new List<SortedSplitNode<T>>();

        private int _count = 0;
        private bool _isDirty;

        public SortedSplitList(IComparer<T> defaultComparer, int deepness = 1000)
        {
            _deepness = deepness;
            _defaultComparer = defaultComparer ?? throw new ArgumentNullException(nameof(defaultComparer));
            _verticalComparer = new CompareByFirstItem<T>(_defaultComparer);
        }

        public int Count => _count;

        public T this[int i]
        {
            get
            {
                if (i < 0) throw new IndexOutOfRangeException("Index < 0");
                ReaderWriterLock.EnterReadLock();
                try
                {
                    RecalculateIndexerIfDirty();
                    var index = GetVerticalTableIndex(default(T), i, null);
                    var beginIndex = _verticalIndex[index].BeginIndex;
                    var currentList = _verticalIndex[index].Items;
                    return currentList[i - beginIndex];
                }
                finally
                {
                    ReaderWriterLock.ExitReadLock();
                }
            }
        }

        public void Add(T item)
        {
            ReaderWriterLock.EnterWriteLock();
            try
            {
                AddOneItem(item);
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }

        public void Remove(T item)
        {
            ReaderWriterLock.EnterWriteLock();
            try
            {
                RemoveOneItem(item);
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }

        public void RemoveAll(Predicate<T> match)
        {
            ReaderWriterLock.EnterWriteLock();
            try
            {
                for (var i = Count - 1; i >= 0; i--)
                {
                    var obj = this[i];
                    if (!match(obj)) continue;
                    RemoveOneItem(obj);
                    _isDirty = false;
                }

                _isDirty = true;
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            ReaderWriterLock.EnterWriteLock();
            try
            {
                foreach (var node in _verticalIndex)
                    node.Items.Clear();

                _verticalIndex.Clear();
                _verticalIndex.TrimExcess();
                _count = 0;
                _isDirty = true;
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }

        public int BinarySearch(T comparisonItem, IComparer<T> comparer = null)
        {
            ReaderWriterLock.EnterReadLock();
            try
            {
                RecalculateIndexerIfDirty();
                var vIndex = GetVerticalTableIndex(comparisonItem, -1, comparer ?? _defaultComparer);
                if (vIndex < 0)
                    return vIndex;
                var begin = _verticalIndex[vIndex].BeginIndex;
                var internalArray = _verticalIndex[vIndex].Items;
                var realIndex = FastBinarySearch(internalArray, comparisonItem, comparer ?? _defaultComparer);
                return (realIndex >= 0) ? realIndex + begin : realIndex - begin;
            }
            finally
            {
                ReaderWriterLock.ExitReadLock();
            }
        }

        public IEnumerable<T> PartiallyEnumerate(T comparisonItem, IComparer<T> comparer = null)
        {
            var currentComparer = comparer ?? _defaultComparer;
            ReaderWriterLock.EnterReadLock();
            try
            {
                var index = BinarySearch(comparisonItem, comparer);
                if (index < 0) yield break;
                for (; index > 0 && currentComparer.Compare(comparisonItem, this[index - 1]) == 0; index--) ;
                for (; index < Count && currentComparer.Compare(comparisonItem, this[index]) == 0; index++)
                    yield return this[index];
            }
            finally
            {
                ReaderWriterLock.ExitReadLock();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            ReaderWriterLock.EnterReadLock();
            try
            {
                /* Copy List in case of modification in other thread */
                var list = _verticalIndex.SelectMany(verticalIndexNode => verticalIndexNode.Items).ToList();
                return list.GetEnumerator();
            }
            finally
            {
                ReaderWriterLock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddOneItem(T item)
        {
            if (_verticalIndex.Count == 0)
            {
                _verticalIndex.Add(new SortedSplitNode<T>(item));
            }
            else
            {
                InternalAdd(item);
            }

            _count++;
            _isDirty = true;
        }

        private void RemoveOneItem(T item)
        {
            var deletedNode = false;
            var vi = GetVerticalTableIndex(item, -1, null);
            var currentTable = _verticalIndex[vi].Items;

            var positionToRemove = FastBinarySearch(currentTable, item, _defaultComparer);
            if (positionToRemove >= 0)
            {
                currentTable.RemoveAt(positionToRemove);

                if (currentTable.Count == 0)
                {
                    _verticalIndex.RemoveAt(vi);
                    deletedNode = true;
                }
                else if (vi > 0 && (_verticalIndex[vi - 1].Items.Count + currentTable.Count) < _deepness)
                {
                    _verticalIndex[vi - 1].Items.AddRange(currentTable);
                    currentTable.Clear();
                    _verticalIndex.RemoveAt(vi);
                    deletedNode = true;
                }

                if (deletedNode == false && positionToRemove == 0)
                    _verticalIndex[vi].FirstItem = _verticalIndex[vi].Items[0];

                _count--;
                _isDirty = true;
            }
            else
                throw new InvalidOperationException("Item Not Found");
        }

        private void InternalAdd(T item)
        {
            var vi = GetVerticalTableIndex(item, -1, null);
            var currentTable = _verticalIndex[vi].Items;

            var position = FastBinarySearch(currentTable, item, _defaultComparer);

            if (position < 0)
            {
                if (~position < currentTable.Count)
                {
                    position = ~position;
                }
                else
                    position = -1;
            }

            if (currentTable.Count < _deepness)
            {
                if (position == 0)
                {
                    _verticalIndex[vi].FirstItem = item;
                    currentTable.Insert(position, item);
                }
                else if (position > 0)
                    currentTable.Insert(position, item);
                else
                    currentTable.Add(item);

                return;
            }

            var median = _deepness >> 1;
            // if latest index or next index cannot take half of current one create a new one
            if (vi == _verticalIndex.Count - 1 || _verticalIndex[vi + 1].Items.Count > median)
            {
                _verticalIndex.Insert(vi + 1, new SortedSplitNode<T>());
            }

            if (position >= 0 && position <= median)
            {
                //insert into first part of the list

                // move second part to next node
                _verticalIndex[vi + 1].Items.InsertRange(0, currentTable.GetRange(median, currentTable.Count - median));
                currentTable.RemoveRange(median, currentTable.Count - median);

                // insert items in x
                currentTable.Insert(position, item);
            }
            else
            {
                // insert into second part of the list

                // insert item in current list
                if (position > 0)
                    currentTable.Insert(position, item);
                else
                    currentTable.Add(item);

                // Move last item to next list
                _verticalIndex[vi + 1].Items.InsertRange(0,
                    currentTable.GetRange(median, currentTable.Count - median));
                currentTable.RemoveRange(median, currentTable.Count - median);
            }

            // update First items
            _verticalIndex[vi + 1].FirstItem = _verticalIndex[vi + 1].Items[0];
            if (position == 0)
                _verticalIndex[vi].FirstItem = _verticalIndex[vi].Items[0];
        }

        private static int FastBinarySearch(IReadOnlyList<T> array, T value, IComparer<T> comparer)
        {
            var num = 0;
            var num2 = array.Count - 1;
            while (num <= num2)
            {
                var num3 = num + ((num2 - num) >> 1);
                var num4 = comparer.Compare(array[num3], value);
                if (num4 == 0)
                {
                    return num3;
                }

                if (num4 < 0)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3 - 1;
                }
            }

            return ~num;
        }

        private int GetVerticalTableIndex(T containItem, int containIndex, IComparer<T> comparer)
        {
            var localStruct = new SortedSplitNode<T>(containItem, containIndex);

            if (comparer != null)
                _verticalComparer.LocalComparer = comparer;

            var index = _verticalIndex.BinarySearch(
                localStruct,
                (containIndex < 0) ? _verticalComparer : (IComparer<SortedSplitNode<T>>) IndexComparer);

            if (comparer != null)
                _verticalComparer.LocalComparer = _defaultComparer;

            if (index >= 0) return index;

            if (~index >= _verticalIndex.Count) return _verticalIndex.Count - 1;
            if (~index <= 1)
                return 0;
            return ~index - 1;
        }

        private void RecalculateIndexerIfDirty()
        {
            if (!_isDirty) return;
            var begin = 0;
            foreach (var item in _verticalIndex)
            {
                item.BeginIndex = begin;
                begin += item.Items.Count;
            }

            _isDirty = false;
        }
    }
}