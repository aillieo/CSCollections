using System;
using System.Collections;
using System.Collections.Generic;

namespace AillieoUtils.Collections
{
    public class MultiDictionary<TKey, TValue> : IDictionary<TKey, ICollection<TValue>>
    {
        private readonly Dictionary<TKey, IList<TValue>> dict;

        public MultiDictionary()
            : this(0, null)
        {
        }

        public MultiDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        public MultiDictionary(int capacity)
            : this(capacity, null)
        {
        }

        public MultiDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            dict = new Dictionary<TKey, IList<TValue>>(capacity, comparer);
        }

        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }

        bool ICollection<KeyValuePair<TKey, ICollection<TValue>>>.IsReadOnly => false;

        public void Add(TKey key, ICollection<TValue> value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out ICollection<TValue> value)
        {
            throw new NotImplementedException();
        }

        public ICollection<TValue> this[TKey key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ICollection<TKey> Keys { get; }

        public ICollection<ICollection<TValue>> Values { get; }
    }
}
