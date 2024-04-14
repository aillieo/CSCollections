// -----------------------------------------------------------------------
// <copyright file="MultiDictionary.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

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
            this.dict = new Dictionary<TKey, IList<TValue>>(capacity, comparer);
        }

        /// <inheritdoc/>
        public int Count { get; }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, ICollection<TValue>>>.IsReadOnly => false;

        /// <inheritdoc/>
        public ICollection<TKey> Keys { get; }

        /// <inheritdoc/>
        public ICollection<ICollection<TValue>> Values { get; }

        /// <inheritdoc/>
        public ICollection<TValue> this[TKey key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Add(TKey key, ICollection<TValue> value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out ICollection<TValue> value)
        {
            throw new NotImplementedException();
        }
    }
}
