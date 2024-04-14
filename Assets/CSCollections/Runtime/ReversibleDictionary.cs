// -----------------------------------------------------------------------
// <copyright file="ReversibleDictionary.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ReversibleDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary;
        private readonly Dictionary<TValue, HashSet<TKey>> lookup;

        public ReversibleDictionary()
            : this(0, null, null)
        {
        }

        public ReversibleDictionary(int capacity)
            : this(capacity, null, null)
        {
        }

        public ReversibleDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer, null)
        {
        }

        public ReversibleDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : this(capacity, comparer, null)
        {
        }

        public ReversibleDictionary(IEqualityComparer<TKey> comparer, IEqualityComparer<TValue> valueComparer)
            : this(0, comparer, valueComparer)
        {
        }

        public ReversibleDictionary(int capacity, IEqualityComparer<TKey> comparer, IEqualityComparer<TValue> valueComparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
            this.lookup = new Dictionary<TValue, HashSet<TKey>>(capacity, valueComparer);
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys => this.dictionary.Keys;

        /// <inheritdoc/>
        public ICollection<TValue> Values => this.dictionary.Values;

        /// <inheritdoc/>
        public int Count => this.dictionary.Count;

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get => this.dictionary[key];
            set
            {
                if (this.dictionary.TryGetValue(key, out TValue v))
                {
                    // replace
                    this.InternalRemoveKeyForValue(v, key);
                    this.dictionary[key] = value;
                    this.InternalAddKeyForValue(value, key);
                }
                else
                {
                    // add
                    this.dictionary.Add(key, value);
                    this.InternalAddKeyForValue(value, key);
                }
            }
        }

        public bool HasKeyForValue(TValue value)
        {
            if (this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                return keys.Count > 0;
            }

            return false;
        }

        public TKey FirstKeyForValue(TValue value)
        {
            if (this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                if (keys.Count > 0)
                {
                    return keys.First();
                }
            }

            throw new KeyNotFoundException();
        }

        public TKey FirstKeyForValueOrDefault(TValue value, TKey defaultKey)
        {
            if (this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                if (keys.Count > 0)
                {
                    return keys.First();
                }
            }

            return defaultKey;
        }

        public IEnumerable<TKey> KeysForValue(TValue value)
        {
            if (this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                if (keys.Count > 0)
                {
                    return keys;
                }
            }

            return default;
        }

        public int RemoveKeysForValue(TValue value)
        {
            var count = 0;
            if (this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                foreach (var k in keys)
                {
                    this.dictionary.Remove(k);
                    count++;
                }

                this.lookup.Remove(value);
            }

            return count;
        }

        public void ClearLight()
        {
            this.dictionary.Clear();
            foreach (var pair in this.lookup)
            {
                pair.Value.Clear();
            }
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
            this.InternalAddKeyForValue(value, key);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.dictionary.Add(item.Key, item.Value);
            this.InternalAddKeyForValue(item.Value, item.Key);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.dictionary.Clear();
            this.lookup.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Contains(item);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            if (this.dictionary.TryGetValue(key, out TValue value))
            {
                this.dictionary.Remove(key);
                this.InternalRemoveKeyForValue(value, key);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Remove(item))
            {
                this.InternalRemoveKeyForValue(item.Value, item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void InternalAddKeyForValue(TValue value, TKey key)
        {
            if (!this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                keys = new HashSet<TKey>();
                this.lookup.Add(value, keys);
            }

            keys.Add(key);
        }

        private void InternalRemoveKeyForValue(TValue value, TKey key)
        {
            if (!this.lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                return;
            }

            keys.Remove(key);
        }
    }
}
