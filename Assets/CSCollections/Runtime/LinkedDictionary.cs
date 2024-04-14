// -----------------------------------------------------------------------
// <copyright file="LinkedDictionary.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, ValueWrapper> dict;
        private int version;

        private TKey firstKey;

        private TKey lastKey;

        public LinkedDictionary()
            : this(0)
        {
        }

        public LinkedDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        public LinkedDictionary(int capacity)
        {
            this.dict = new Dictionary<TKey, ValueWrapper>(capacity);
        }

        public LinkedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.dict = new Dictionary<TKey, ValueWrapper>(capacity, comparer);
        }

        public TKey FirstKey
        {
            get
            {
                if (this.dict.Count == 0)
                {
                    throw new KeyNotFoundException(string.Empty);
                }

                return this.firstKey;
            }
        }

        public TKey LastKey
        {
            get
            {
                if (this.dict.Count == 0)
                {
                    throw new KeyNotFoundException(string.Empty);
                }

                return this.lastKey;
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys => this.Select(pair => pair.Key).ToArray();

        /// <inheritdoc/>
        public ICollection<TValue> Values => this.Select(pair => pair.Value).ToArray();

        /// <inheritdoc/>
        public int Count => this.dict.Count;

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get => this.dict[key].value;
            set
            {
                if (this.dict.TryGetValue(key, out ValueWrapper wrapper))
                {
                    wrapper.value = value;
                    this.version++;
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        public void AddLast(TKey key, TValue value)
        {
            if (this.dict.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            var wrapper = ValueWrapper.Wrap(value);
            if (this.dict.Count == 0)
            {
                this.firstKey = key;
                this.lastKey = key;
                wrapper.nextKey = key;
                wrapper.previousKey = key;
            }
            else
            {
                ValueWrapper first = this.dict[this.firstKey];
                ValueWrapper last = this.dict[this.lastKey];
                first.previousKey = key;
                last.nextKey = key;
                wrapper.previousKey = this.lastKey;
                wrapper.nextKey = this.firstKey;
                this.lastKey = key;
            }

            this.InternalAdd(key, wrapper);
        }

        public void AddFirst(TKey key, TValue value)
        {
            if (this.dict.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            var wrapper = ValueWrapper.Wrap(value);
            if (this.dict.Count == 0)
            {
                this.firstKey = key;
                this.lastKey = key;
                wrapper.nextKey = key;
                wrapper.previousKey = key;
            }
            else
            {
                ValueWrapper first = this.dict[this.firstKey];
                ValueWrapper last = this.dict[this.lastKey];
                first.previousKey = key;
                last.nextKey = key;
                wrapper.previousKey = this.lastKey;
                wrapper.nextKey = this.firstKey;
                this.firstKey = key;
            }

            this.InternalAdd(key, wrapper);
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            this.AddLast(key, value);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.dict.ContainsKey(key);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            if (this.dict.TryGetValue(key, out ValueWrapper wrapper))
            {
                return this.InternalRemove(key, wrapper);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            if (this.dict.TryGetValue(key, out ValueWrapper wrapper))
            {
                value = wrapper.value;
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
            foreach (var pair in this.dict)
            {
                ValueWrapper.Recycle(pair.Value);
            }

            this.firstKey = this.lastKey = default;
            this.dict.Clear();
            this.version++;
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (this.dict.TryGetValue(item.Key, out ValueWrapper wrapper))
            {
                return EqualityComparer<TValue>.Default.Equals(wrapper.value, item.Value);
            }

            return false;
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), $"invalid argument {nameof(array)}");
            }

            if (arrayIndex < 0 || array.Length + arrayIndex < this.dict.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), $"invalid argument {nameof(arrayIndex)}");
            }

            var data = this.dict.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.value)).ToArray();
            Array.Copy(data, 0, array, arrayIndex, data.Length);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this.dict.TryGetValue(item.Key, out ValueWrapper wrapper) &&
                EqualityComparer<TValue>.Default.Equals(wrapper.value, item.Value))
            {
                return this.InternalRemove(item.Key, wrapper);
            }

            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var v = this.version;
            if (this.dict.Count > 0)
            {
                TKey key = this.firstKey;
                while (true)
                {
                    if (this.version != v)
                    {
                        throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                    }

                    ValueWrapper wrapper = this.dict[key];
                    yield return new KeyValuePair<TKey, TValue>(key, wrapper.value);
                    key = wrapper.nextKey;

                    if (this.dict.Comparer.Equals(key, this.firstKey))
                    {
                        break;
                    }
                }
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void InternalAdd(TKey key, ValueWrapper wrapper)
        {
            this.dict.Add(key, wrapper);
            this.version++;
        }

        private bool InternalRemove(TKey key, ValueWrapper wrapper)
        {
            if (this.dict.Count == 1)
            {
                this.firstKey = default;
                this.lastKey = default;
            }
            else
            {
                if (this.dict.TryGetValue(wrapper.nextKey, out ValueWrapper next))
                {
                    next.previousKey = wrapper.previousKey;
                }

                if (this.dict.TryGetValue(wrapper.previousKey, out ValueWrapper previous))
                {
                    previous.nextKey = wrapper.nextKey;
                }

                if (this.dict.Comparer.Equals(this.firstKey, key))
                {
                    this.firstKey = wrapper.nextKey;
                }

                if (this.dict.Comparer.Equals(this.lastKey, key))
                {
                    this.lastKey = wrapper.previousKey;
                }
            }

            ValueWrapper.Recycle(wrapper);
            this.dict.Remove(key);

            this.version++;
            return true;
        }

        [Serializable]
        private class ValueWrapper
        {
            public TValue value;
            public TKey nextKey;
            public TKey previousKey;
            private static readonly Stack<ValueWrapper> pool = new Stack<ValueWrapper>();

            public static ValueWrapper Wrap(TValue val)
            {
                if (pool.Count > 0)
                {
                    ValueWrapper wrapper = pool.Pop();
                    wrapper.value = val;
                    return wrapper;
                }

                return new ValueWrapper() { value = val };
            }

            public static void Recycle(ValueWrapper wrapper)
            {
                pool.Push(wrapper);
            }
        }
    }
}
