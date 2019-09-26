using System.Collections.Generic;

namespace Small.Net.Collection
{
    public class WithSortDirectionCompare<T> : IComparer<T>
    {
        private readonly bool _isMax;
        private readonly IComparer<T> _comparer;

        internal WithSortDirectionCompare(SortDirection sortDirection, IComparer<T> comparer)
        {
            this._isMax = sortDirection == SortDirection.Descending;
            this._comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return !_isMax ? _comparer.Compare(x, y) : _comparer.Compare(y, x);
        }
    }
}