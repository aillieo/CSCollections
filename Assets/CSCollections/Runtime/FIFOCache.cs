// -----------------------------------------------------------------------
// <copyright file="FIFOCache.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a First-In-First-Out (FIFO) cache implementation using a linked dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the cache.</typeparam>
    /// <typeparam name="TValue">The type of the values in the cache.</typeparam>
    public class FIFOCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static readonly int defaultCapacity = 255;
        private readonly LinkedDictionary<TKey, TValue> linkedDictionary;
        private int capacity;

        public FIFOCache(int capacity)
            : this(capacity, null)
        {
        }

        public FIFOCache()
            : this(defaultCapacity)
        {
        }

        public FIFOCache(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.capacity = capacity;
            this.linkedDictionary = new LinkedDictionary<TKey, TValue>(capacity, null);
        }

        public FIFOCache(IEqualityComparer<TKey> comparer)
            : this(defaultCapacity, comparer)
        {
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys => this.linkedDictionary.Keys;

        /// <inheritdoc/>
        public ICollection<TValue> Values => this.linkedDictionary.Values;

        /// <inheritdoc/>
        public int Count => this.linkedDictionary.Count;

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                return this.linkedDictionary[key];
            }

            set
            {
                if (this.linkedDictionary.Remove(key))
                {
                    this.linkedDictionary.AddLast(key, value);
                }
                else
                {
                    if (this.linkedDictionary.Count + 1 > this.capacity)
                    {
                        this.linkedDictionary.Remove(this.linkedDictionary.FirstKey);
                    }

                    this.linkedDictionary.AddLast(key, value);
                }
            }
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            this.linkedDictionary.AddLast(key, value);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.linkedDictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            return this.linkedDictionary.Remove(key);
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.linkedDictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.linkedDictionary.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.linkedDictionary).Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.linkedDictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.linkedDictionary).Remove(item);
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
