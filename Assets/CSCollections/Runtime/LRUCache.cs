// -----------------------------------------------------------------------
// <copyright file="LRUCache.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class LRUCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static readonly int defaultCapacity = 255;
        private readonly LinkedDictionary<TKey, TValue> linkedDictionary;
        private int capacity;

        public LRUCache()
            : this(defaultCapacity)
        {
        }

        public LRUCache(int capacity)
            : this(capacity, null)
        {
        }

        public LRUCache(IEqualityComparer<TKey> comparer)
            : this(defaultCapacity, comparer)
        {
        }

        public LRUCache(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), $"invalid argument {nameof(capacity)}");
            }

            this.capacity = capacity;
            this.linkedDictionary = new LinkedDictionary<TKey, TValue>(capacity, comparer);
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                return this.linkedDictionary.Count;
            }
        }

        public int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                if (value > 0 && this.capacity != value)
                {
                    this.capacity = value;
                    while (this.linkedDictionary.Count > this.capacity)
                    {
                        this.linkedDictionary.Remove(this.linkedDictionary.LastKey);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys => this.Select(pair => pair.Key).ToList().AsReadOnly();

        /// <inheritdoc/>
        public ICollection<TValue> Values => this.Select(pair => pair.Value).ToList().AsReadOnly();

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                if (this.TryGetValue(key, out TValue value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }

            set
            {
                this.linkedDictionary.Remove(key);
                this.linkedDictionary.AddFirst(key, value);
                if (this.linkedDictionary.Count > this.capacity)
                {
                    this.linkedDictionary.Remove(this.linkedDictionary.LastKey);
                }
            }
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            return this.linkedDictionary.Remove(key);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.linkedDictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            if (this.linkedDictionary.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            this.linkedDictionary.Add(key, value);
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.linkedDictionary.TryGetValue(key, out value))
            {
                this.linkedDictionary.Remove(key);
                this.linkedDictionary.AddFirst(key, value);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.linkedDictionary.Clear();
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.linkedDictionary.Contains(item);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.linkedDictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.linkedDictionary.Remove(item.Key);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.linkedDictionary.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
