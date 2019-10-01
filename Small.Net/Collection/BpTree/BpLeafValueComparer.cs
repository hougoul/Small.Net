using System;
using System.Collections.Generic;

namespace Small.Net.Collection
{
    public class BpLeafValueComparer<TKey, TValue> : IComparer<TValue>
    {
        private readonly Func<TValue, TKey> _keyExtractor;
        private readonly IComparer<TKey> _keyComparer;

        public BpLeafValueComparer(Func<TValue, TKey> keyExtractor, IComparer<TKey> keyComparer)
        {
            _keyExtractor = keyExtractor;
            _keyComparer = keyComparer;
        }

        public int Compare(TValue x, TValue y)
        {
            if (x == null) return -1;
            if (y == null) return 1;
            var xKey = _keyExtractor(x);
            if (xKey == null) return -1;
            var yKey = _keyExtractor(y);
            return yKey == null ? 1 : _keyComparer.Compare(xKey, yKey);
        }
    }
}