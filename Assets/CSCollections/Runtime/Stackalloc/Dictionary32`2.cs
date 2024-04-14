// -----------------------------------------------------------------------
// <copyright file="Dictionary32`2.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public struct Dictionary32<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private List32<KeyValuePair<TKey, TValue>> entries;

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get
            {
                var keys = new List<TKey>(this.Count);
                foreach (var entry in this.entries)
                {
                    keys.Add(entry.Key);
                }

                return keys;
            }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get
            {
                var values = new List<TValue>(this.Count);
                foreach (var entry in this.entries)
                {
                    values.Add(entry.Value);
                }

                return values;
            }
        }

        /// <inheritdoc/>
        public int Count => this.entries.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                foreach (var entry in this.entries)
                {
                    if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                    {
                        return entry.Value;
                    }
                }

                throw new KeyNotFoundException();
            }

            set
            {
                for (var i = 0; i < this.entries.Count; i++)
                {
                    if (EqualityComparer<TKey>.Default.Equals(this.entries[i].Key, key))
                    {
                        this.entries[i] = new KeyValuePair<TKey, TValue>(key, value);
                        return;
                    }
                }

                this.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                throw new System.ArgumentException("An entry with the same key already exists.");
            }

            this.entries.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.entries.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            foreach (var entry in this.entries)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, item.Key) && EqualityComparer<TValue>.Default.Equals(entry.Value, item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            foreach (var entry in this.entries)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex + this.Count > array.Length)
            {
                throw new System.ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            for (var i = 0; i < this.entries.Count; i++)
            {
                array[arrayIndex + i] = this.entries[i];
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.entries.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            for (var i = 0; i < this.entries.Count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(this.entries[i].Key, key))
                {
                    this.entries.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            for (var i = 0; i < this.entries.Count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(this.entries[i].Key, item.Key) && EqualityComparer<TValue>.Default.Equals(this.entries[i].Value, item.Value))
                {
                    this.entries.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var entry in this.entries)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
