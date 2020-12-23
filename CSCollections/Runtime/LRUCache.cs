using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class LRUCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static readonly int defaultCapacity = 255;
        private int capacity;
        private readonly LinkedDictionary<TKey, TValue> linkedDictionary;

        public LRUCache() : this(defaultCapacity) { }

        public LRUCache(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException($"invalid argument {nameof(capacity)}");
            }

            this.capacity = capacity;
            linkedDictionary = new LinkedDictionary<TKey, TValue>(capacity);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }

            set
            {
                linkedDictionary.Remove(key);
                linkedDictionary.AddFirst(key, value);
                if (linkedDictionary.Count > capacity)
                {
                    linkedDictionary.Remove(linkedDictionary.LastKey);
                }
            }
        }

        public bool Remove(TKey key)
        {
            return linkedDictionary.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return linkedDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            if (linkedDictionary.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            linkedDictionary.Add(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (linkedDictionary.TryGetValue(key, out value))
            {
                linkedDictionary.Remove(key);
                linkedDictionary.AddFirst(key, value);
                return true;
            }

            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            linkedDictionary.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return linkedDictionary.Count;
            }
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (value > 0 && capacity != value)
                {
                    capacity = value;
                    while (linkedDictionary.Count > capacity)
                    {
                        linkedDictionary.Remove(linkedDictionary.LastKey);
                    }
                }
            }
        }

        public ICollection<TKey> Keys => this.Select(pair => pair.Key).ToList().AsReadOnly();

        public ICollection<TValue> Values => this.Select(pair => pair.Value).ToList().AsReadOnly();

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
    }
}
