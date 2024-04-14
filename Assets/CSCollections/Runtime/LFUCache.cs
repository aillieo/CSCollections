// -----------------------------------------------------------------------
// <copyright file="LFUCache.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a Least Frequently Used (LFU) cache implementation.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache keys.</typeparam>
    /// <typeparam name="TValue">The type of the cache values.</typeparam>
    public class LFUCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static readonly float defaultShrinkFactor = 0.75f;
        private static readonly int defaultCapacity = 255;

        private readonly Dictionary<TKey, ValueWrapper> dictionary;
        private readonly LinkedDictionary<TKey, ValueWrapper>[] dictByFrequency;
        private readonly int maxFrequency;
        private readonly int capacity;
        private readonly float shrinkFactor;

        private int lowestFrequency;

        /// <summary>
        /// Initializes a new instance of the <see cref="LFUCache{TKey, TValue}"/> class with the default capacity and shrink factor.
        /// </summary>
        public LFUCache()
            : this(defaultCapacity, defaultShrinkFactor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LFUCache{TKey, TValue}"/> class with the specified capacity and the default shrink factor.
        /// </summary>
        /// <param name="capacity">The maximum number of items that the cache can hold.</param>
        public LFUCache(int capacity)
            : this(capacity, defaultShrinkFactor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LFUCache{TKey, TValue}"/> class with the specified capacity and shrink factor.
        /// </summary>
        /// <param name="capacity">The maximum number of items that the cache can hold.</param>
        /// <param name="shrinkFactor">The factor used to determine when to shrink the cache.</param>
        public LFUCache(int capacity, float shrinkFactor)
            : this(capacity, null, shrinkFactor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LFUCache{TKey, TValue}"/> class with the default capacity, the specified comparer, and the default shrink factor.
        /// </summary>
        /// <param name="comparer">The comparer used to determine equality of keys.</param>
        public LFUCache(IEqualityComparer<TKey> comparer)
            : this(defaultCapacity, comparer, defaultShrinkFactor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LFUCache{TKey, TValue}"/> class with the specified capacity, comparer, and shrink factor.
        /// </summary>
        /// <param name="capacity">The maximum number of items that the cache can hold.</param>
        /// <param name="comparer">The comparer used to determine equality of keys.</param>
        /// <param name="shrinkFactor">The factor used to determine when to shrink the cache.</param>
        public LFUCache(int capacity, IEqualityComparer<TKey> comparer, float shrinkFactor)
        {
            if (shrinkFactor <= 0 || shrinkFactor >= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(shrinkFactor), $"invalid argument {nameof(shrinkFactor)}: expected range (0,1)");
            }

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), $"invalid argument {nameof(capacity)}");
            }

            this.dictionary = new Dictionary<TKey, ValueWrapper>(capacity);
            this.dictByFrequency = new LinkedDictionary<TKey, ValueWrapper>[capacity];
            this.lowestFrequency = 0;
            this.maxFrequency = capacity - 1;
            this.capacity = capacity;
            this.shrinkFactor = shrinkFactor;
        }

        /// <inheritdoc/>
        public int Count => this.dictionary.Count;

        /// <summary>
        /// Gets the maximum number of items that the cache can hold.
        /// </summary>
        public int Capacity => this.capacity;

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
                TValue value = default;
                if (this.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }

            set
            {
                ValueWrapper wrapper = default;
                if (!this.dictionary.TryGetValue(key, out wrapper))
                {
                    this.Add(key, value);
                }
                else
                {
                    wrapper.value = value;
                }
            }
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            if (this.dictionary.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            if (this.dictionary.Count == this.capacity)
            {
                this.Shrink();
            }

            LinkedDictionary<TKey, ValueWrapper> dict = this.GetDictByFrequency(0);
            var wrapper = new ValueWrapper(value, 0);
            dict.Add(key, wrapper);
            this.dictionary[key] = wrapper;
            this.lowestFrequency = 0;
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            ValueWrapper wrapper = default;
            if (this.dictionary.TryGetValue(key, out wrapper))
            {
                LinkedDictionary<TKey, ValueWrapper> dict = this.GetDictByFrequency(wrapper.frequency);
                dict.Remove(key);
                if (this.lowestFrequency == wrapper.frequency)
                {
                    this.UpdateLowestFrequency();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            for (var i = 0; i <= this.maxFrequency; i++)
            {
                if (this.dictByFrequency[i] != null)
                {
                    this.dictByFrequency[i].Clear();
                }
            }

            this.dictionary.Clear();
            this.lowestFrequency = 0;
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            ValueWrapper wrapper = default;
            if (this.dictionary.TryGetValue(key, out wrapper))
            {
                var currentFrequency = wrapper.frequency;
                if (currentFrequency < this.maxFrequency)
                {
                    var nextFrequency = currentFrequency + 1;

                    LinkedDictionary<TKey, ValueWrapper> curDict = this.GetDictByFrequency(currentFrequency);
                    LinkedDictionary<TKey, ValueWrapper> nextDict = this.GetDictByFrequency(nextFrequency);

                    curDict.Remove(key);
                    nextDict.Add(key, wrapper);
                    wrapper.frequency = nextFrequency;

                    this.dictionary[key] = wrapper;
                    if (this.lowestFrequency == currentFrequency && curDict.Count == 0)
                    {
                        this.lowestFrequency = nextFrequency;
                    }
                }
                else
                {
                    // f相同时 用LRU策略
                    LinkedDictionary<TKey, ValueWrapper> dict = this.GetDictByFrequency(currentFrequency);
                    dict.Remove(key);
                    dict.Add(key, wrapper);
                }

                value = wrapper.value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private LinkedDictionary<TKey, ValueWrapper> GetDictByFrequency(int frequency)
        {
            if (frequency < 0 || frequency > this.maxFrequency)
            {
                throw new Exception();
            }

            if (this.dictByFrequency[frequency] == null)
            {
                this.dictByFrequency[frequency] = new LinkedDictionary<TKey, ValueWrapper>();
            }

            return this.dictByFrequency[frequency];
        }

        private void Shrink()
        {
            var removed = 0;
            var toRemove = this.dictionary.Count - (int)(this.shrinkFactor * this.capacity);
            while (removed < toRemove)
            {
                LinkedDictionary<TKey, ValueWrapper> dict = this.GetDictByFrequency(this.lowestFrequency);
                if (dict.Count == 0)
                {
                    throw new Exception();
                }

                while (dict.Count > 0 && removed < toRemove)
                {
                    var key = dict.FirstKey;

                    var r = dict.Remove(key);

                    this.dictionary.Remove(key);

                    ++removed;
                }

                if (dict.Count == 0)
                {
                    this.UpdateLowestFrequency();
                }
            }
        }

        private void UpdateLowestFrequency()
        {
            while (this.lowestFrequency <= this.maxFrequency && this.GetDictByFrequency(this.lowestFrequency).Count == 0)
            {
                this.lowestFrequency++;
            }

            if (this.lowestFrequency > this.maxFrequency)
            {
                this.lowestFrequency = 0;
            }
        }

        [Serializable]
        private class ValueWrapper
        {
            public TValue value;
            public int frequency;

            public ValueWrapper(TValue value, int frequency)
            {
                this.value = value;
                this.frequency = frequency;
            }
        }
    }
}
