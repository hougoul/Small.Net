using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Small.Net.Collection
{
    public class BinaryHeap<T> : ICollection<T>, IEnumerable<T>
    {
        private readonly bool _isMaxHeap;

        private T[] _heapArray;
        private readonly IComparer<T> _comparer;

        public int Count { get; private set; }
        public bool IsReadOnly { get; } = false;

        public BinaryHeap(IComparer<T> comparer)
            : this(SortDirection.Ascending, null, comparer)
        {
        }

        public BinaryHeap(IEnumerable<T> initial, IComparer<T> comparer)
            : this(SortDirection.Ascending, initial, comparer)
        {
        }

        public BinaryHeap(SortDirection sortDirection, IComparer<T> comparer)
            : this(sortDirection, null, comparer)
        {
        }

        /// <summary>
        /// Time complexity: O(n) if initial is provided. Otherwise O(1).
        /// </summary>
        /// <param name="initial">The initial items in the heap.</param>
        public BinaryHeap(SortDirection sortDirection, IEnumerable<T> initial, IComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            this._isMaxHeap = sortDirection == SortDirection.Descending;
            this._comparer = new WithSortDirectionCompare<T>(sortDirection, comparer);

            if (initial != null)
            {
                var items = initial as T[] ?? initial.ToArray();
                var initArray = new T[items.Count()];
                items.CopyTo(initArray, 0);
                BulkInit(initArray);
                Count = initArray.Length;
            }
            else
            {
                _heapArray = new T[2];
            }
        }


        /// <summary>
        /// Time complexity: O(log(n)).
        /// </summary>
        public void Add(T newItem)
        {
            if (Count == _heapArray.Length)
            {
                DoubleArray();
            }

            _heapArray[Count] = newItem;

            for (var i = Count; i > 0; i = (i - 1) >> 1)
            {
                var index = (i - 1) >> 1;
                if (_comparer.Compare(_heapArray[i], _heapArray[index]) >= 0)
                    break;

                var temp = _heapArray[index];
                _heapArray[index] = _heapArray[i];
                _heapArray[i] = temp;
            }

            Count++;
        }

        public void Clear()
        {
            _heapArray = new T[2];
            Count = 0;
        }

        /// <summary>
        /// Time complexity: O(log(n)).
        /// </summary>
        public T Extract()
        {
            if (Count == 0)
            {
                throw new Exception("Empty heap");
            }

            var minMax = _heapArray[0];

            RemoveAt(0);

            return minMax;
        }

        /// <summary>
        /// Time complexity: O(1).
        /// </summary>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new Exception("Empty heap");
            }

            return _heapArray[0];
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex > Count)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), $"0 <= Index  < {Count}");
            _heapArray.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Time complexity: O(n).
        /// </summary>
        public bool Remove(T value)
        {
            var index = FindIndex(value);

            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Time complexity: O(n).
        /// </summary>
        public bool Contains(T value)
        {
            return FindIndex(value) != -1;
        }


        private void BulkInit(T[] initial)
        {
            var i = (initial.Length - 1) >> 1;

            while (i >= 0)
            {
                BulkInitRecursive(i, initial);
                i--;
            }

            _heapArray = initial;
        }

        private void BulkInitRecursive(int i, T[] initial)
        {
            while (true)
            {
                var parent = i;

                var left = 2 * i + 1;
                var right = 2 * i + 2;

                var minMax = left < initial.Length && right < initial.Length
                    ? _comparer.Compare(initial[left], initial[right]) < 0 ? left : right
                    : left < initial.Length
                        ? left
                        : right < initial.Length
                            ? right
                            : -1;

                if (minMax != -1 && _comparer.Compare(initial[minMax], initial[parent]) < 0)
                {
                    var temp = initial[minMax];
                    initial[minMax] = initial[parent];
                    initial[parent] = temp;

                    //drill down to child
                    i = minMax;
                    continue;
                }

                break;
            }
        }

        private void RemoveAt(int parentIndex)
        {
            _heapArray[parentIndex] = _heapArray[Count - 1];
            Count--;

            //percolate down
            while (true)
            {
                var leftIndex = 2 * parentIndex + 1;
                var rightIndex = 2 * parentIndex + 2;

                var parent = _heapArray[parentIndex];

                if (leftIndex < Count && rightIndex < Count)
                {
                    var leftChild = _heapArray[leftIndex];
                    var rightChild = _heapArray[rightIndex];

                    var leftIsMinMax = _comparer.Compare(leftChild, rightChild) < 0;

                    var minMaxChildIndex = leftIsMinMax ? leftIndex : rightIndex;

                    if (_comparer.Compare(_heapArray[minMaxChildIndex], parent) >= 0)
                    {
                        break;
                    }

                    var temp = _heapArray[parentIndex];
                    _heapArray[parentIndex] = _heapArray[minMaxChildIndex];
                    _heapArray[minMaxChildIndex] = temp;

                    parentIndex = leftIsMinMax ? leftIndex : rightIndex;
                }
                else if (leftIndex < Count)
                {
                    if (_comparer.Compare(_heapArray[leftIndex], parent) >= 0)
                    {
                        break;
                    }

                    var temp = _heapArray[parentIndex];
                    _heapArray[parentIndex] = _heapArray[leftIndex];
                    _heapArray[leftIndex] = temp;

                    parentIndex = leftIndex;
                }
                else if (rightIndex < Count)
                {
                    if (_comparer.Compare(_heapArray[rightIndex], parent) >= 0)
                    {
                        break;
                    }

                    var temp = _heapArray[parentIndex];
                    _heapArray[parentIndex] = _heapArray[rightIndex];
                    _heapArray[rightIndex] = temp;

                    parentIndex = rightIndex;
                }
                else
                {
                    break;
                }
            }

            if (_heapArray.Length >> 1 == Count && _heapArray.Length > 2)
            {
                HalfArray();
            }
        }


        private int FindIndex(T value)
        {
            for (var i = 0; i < Count; i++)
            {
                if (_heapArray[i].Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        private void HalfArray()
        {
            var smallerArray = new T[_heapArray.Length >> 1];

            for (var i = 0; i < Count; i++)
            {
                smallerArray[i] = _heapArray[i];
            }

            _heapArray = smallerArray;
        }

        private void DoubleArray()
        {
            var biggerArray = new T[_heapArray.Length * 2];

            for (var i = 0; i < Count; i++)
            {
                biggerArray[i] = _heapArray[i];
            }

            _heapArray = biggerArray;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _heapArray.Take(Count).GetEnumerator();
        }
    }
}