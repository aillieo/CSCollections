using System;
using System.Collections;
using System.Collections.Generic;

namespace AillieoUtils.Collections
{
    public class FIFOCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly LinkedDictionary<TKey, TValue> linkedDictionary;
        private static readonly int defaultCapacity = 255;
        private int capacity;

        public FIFOCache(int capacity)
        {
            this.capacity = capacity;
            this.linkedDictionary = new LinkedDictionary<TKey, TValue>();
        }

        public FIFOCache()
            : this(defaultCapacity)
        {
        }

        public FIFOCache(int capacity, IEqualityComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public FIFOCache(IEqualityComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public ICollection<TKey> Keys => linkedDictionary.Keys;

        public ICollection<TValue> Values => linkedDictionary.Values;

        public int Count => linkedDictionary.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                return linkedDictionary[key];
            }

            set
            {
                if (linkedDictionary.Remove(key))
                {
                    linkedDictionary.AddLast(key, value);
                }
                else
                {
                    if (linkedDictionary.Count + 1 > capacity)
                    {
                        linkedDictionary.Remove(linkedDictionary.FirstKey);
                    }

                    linkedDictionary.AddLast(key, value);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            linkedDictionary.AddLast(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return linkedDictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return linkedDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return linkedDictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            linkedDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)linkedDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return linkedDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
