using System.Collections.Generic;

namespace Small.Net.Collection
{
    internal sealed class SortedSplitNode<T>
    {
        // For performance reason direct public variable
        public T FirstItem;

        // For performance reason direct public variable
        public int BeginIndex;

        // For performance reason direct public variable
        public List<T> Items;

        public SortedSplitNode()
        {
            Items = new List<T>();
        }

        public SortedSplitNode(T firstItem, int beginIndex = 0) : this()
        {
            FirstItem = firstItem;
            Items.Add(firstItem);
            BeginIndex = beginIndex;
        }
    }

    internal sealed class CompareByFirstItem<T> : IComparer<SortedSplitNode<T>>
    {
        public IComparer<T> LocalComparer = null;

        public CompareByFirstItem(IComparer<T> defaultComparer)
        {
            LocalComparer = defaultComparer;
        }

        public int Compare(SortedSplitNode<T> x, SortedSplitNode<T> y)
        {
            if (x == null) return 1;
            if (y == null) return -1;
            return LocalComparer.Compare(x.FirstItem, y.FirstItem);
        }
    }

    internal sealed class CompareByBeginIndex<T> : IComparer<SortedSplitNode<T>>
    {
        public int Compare(SortedSplitNode<T> x, SortedSplitNode<T> y)
        {
            if (x == null) return 1;
            if (y == null) return -1;
            return x.BeginIndex - y.BeginIndex;
        }
    }
}