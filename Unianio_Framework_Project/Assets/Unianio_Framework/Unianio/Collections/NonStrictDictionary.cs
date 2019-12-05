using System.Collections;
using System.Collections.Generic;

namespace Unianio.Collections
{
    public class NonStrictDictionary<TKey,TValue> : IDictionary<TKey, TValue>
    {
        readonly IDictionary<TKey, TValue> _dictionary;
        public NonStrictDictionary() : this((IEqualityComparer<TKey>)null) { }
        public NonStrictDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(KeyValuePair<TKey, TValue> item) => _dictionary[item.Key] = item.Value;
        public void Clear() => _dictionary.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.ContainsKey(item.Key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);
        public int Count => _dictionary.Count;
        public bool IsReadOnly => _dictionary.IsReadOnly;
        public void Add(TKey key, TValue value) => _dictionary[key] = value;
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        public bool Remove(TKey key) => _dictionary.Remove(key);
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
        public TValue this[TKey key]
        {
            get => _dictionary.TryGetValue(key, out var val) ? val : default(TValue);
            set => _dictionary[key] = value;
        }
        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
    }
}