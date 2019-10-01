using System;
using System.Collections.Generic;

namespace Small.Net.Collection
{
    internal struct BpTreeOperation<TKey, TValue>
    {
        public static BpTreeOperation<TKey, TValue> Empty =
            new BpTreeOperation<TKey, TValue>(default(TKey), default(TValue), Comparer<TKey>.Default, null);

        public IComparer<TKey> Comparer;
        public TKey Key;
        public TValue Value;
        public Func<TValue, TKey> ExtractKey;

        public BpTreeOperation(
            TKey keys,
            TValue value,
            IComparer<TKey> comparer,
            Func<TValue, TKey> extractKey)
        {
            Comparer = comparer;
            Key = keys;
            Value = value;
            ExtractKey = extractKey;
        }
    }
}