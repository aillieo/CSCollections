// -----------------------------------------------------------------------
// <copyright file="DualKeyDictionary.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public struct KeyPack<TKey1, TKey2> : IEquatable<KeyPack<TKey1, TKey2>>
    {
        public readonly TKey1 key1;
        public readonly TKey2 key2;

        public KeyPack(TKey1 key1, TKey2 key2)
        {
            this.key1 = key1;
            this.key2 = key2;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is KeyPack<TKey1, TKey2> key)
            {
                return this.Equals(key);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.key1.GetHashCode() * 397 ^ this.key2.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(KeyPack<TKey1, TKey2> other)
        {
            return EqualityComparer<TKey1>.Default.Equals(this.key1, other.key1)
                   && EqualityComparer<TKey2>.Default.Equals(this.key2, other.key2);
        }
    }

    public class DualKeyDictionary<TKey1, TKey2, TValue> : IDictionary<KeyPack<TKey1, TKey2>, TValue>
    {
        private readonly Dictionary<KeyPack<TKey1, TKey2>, TValue> dict;

        public DualKeyDictionary()
        : this(0)
        {
        }

        public DualKeyDictionary(int capacity)
            : this(capacity, null)
        {
        }

        public DualKeyDictionary(int capacity, IEqualityComparer<TKey1> key1Comparer, IEqualityComparer<TKey2> key2Comparer)
            : this(capacity, new EqualityComparer(key1Comparer, key2Comparer))
        {
        }

        public DualKeyDictionary(int capacity, IEqualityComparer<KeyPack<TKey1, TKey2>> comparer)
        {
            this.dict = new Dictionary<KeyPack<TKey1, TKey2>, TValue>(capacity, comparer);
        }

        public DualKeyDictionary(IEqualityComparer<TKey1> key1Comparer, IEqualityComparer<TKey2> key2Comparer)
            : this(0, new EqualityComparer(key1Comparer, key2Comparer))
        {
        }

        public DualKeyDictionary(IEqualityComparer<KeyPack<TKey1, TKey2>> comparer)
            : this(0, comparer)
        {
        }

        /// <inheritdoc/>
        public ICollection<KeyPack<TKey1, TKey2>> Keys => this.dict.Keys;

        public ICollection<TKey1> AllKey1 => this.dict.Select(kvp => kvp.Key.key1).Distinct().ToArray();

        public ICollection<TKey2> AllKey2 => this.dict.Select(kvp => kvp.Key.key2).Distinct().ToArray();

        /// <inheritdoc/>
        public ICollection<TValue> Values => this.dict.Values;

        /// <inheritdoc/>
        public int Count => this.dict.Count;

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>.IsReadOnly => false;

        /// <inheritdoc/>
        public TValue this[KeyPack<TKey1, TKey2> key]
        {
            get => this.dict[key];
            set => this.dict[key] = value;
        }

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get => this.dict[new KeyPack<TKey1, TKey2>(key1, key2)];
            set => this.dict[new KeyPack<TKey1, TKey2>(key1, key2)] = value;
        }

        /// <inheritdoc/>
        public void Add(KeyPack<TKey1, TKey2> key, TValue value)
        {
            this.dict.Add(key, value);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<KeyPack<TKey1, TKey2>, TValue> item)
        {
            ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)this.dict).Add(item);
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            this.dict.Add(new KeyPack<TKey1, TKey2>(key1, key2), value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.dict.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<KeyPack<TKey1, TKey2>, TValue> item)
        {
            return ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)this.dict).Contains(item);
        }

        /// <inheritdoc/>
        public bool ContainsKey(KeyPack<TKey1, TKey2> key)
        {
            return this.dict.ContainsKey(key);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            return this.dict.ContainsKey(new KeyPack<TKey1, TKey2>(key1, key2));
        }

        public bool ContainsKey1(TKey1 key1)
        {
            return this.dict.Any(kvp => kvp.Key.key1.Equals(key1));
        }

        public bool ContainsKey2(TKey2 key2)
        {
            return this.dict.Any(kvp => kvp.Key.key2.Equals(key2));
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<KeyPack<TKey1, TKey2>, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)this.dict).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>> GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(KeyPack<TKey1, TKey2> key)
        {
            return this.dict.Remove(key);
        }

        public bool Remove(TKey1 key1, TKey2 key2)
        {
            return this.dict.Remove(new KeyPack<TKey1, TKey2>(key1, key2));
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<KeyPack<TKey1, TKey2>, TValue> item)
        {
            return ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)this.dict).Remove(item);
        }

        /// <inheritdoc/>
        public bool TryGetValue(KeyPack<TKey1, TKey2> key, out TValue value)
        {
            return this.dict.TryGetValue(key, out value);
        }

        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
        {
            return this.dict.TryGetValue(new KeyPack<TKey1, TKey2>(key1, key2), out value);
        }

        public IEnumerable<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>> EnumKey1(TKey1 key1)
        {
            return this.dict.Where(kvp => kvp.Key.key1.Equals(key1));
        }

        public IEnumerable<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>> EnumKey2(TKey2 key2)
        {
            return this.dict.Where(kvp => kvp.Key.key2.Equals(key2));
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public class EqualityComparer : IEqualityComparer<KeyPack<TKey1, TKey2>>
        {
            private readonly IEqualityComparer<TKey1> equalityComparer1;
            private readonly IEqualityComparer<TKey2> equalityComparer2;

            public EqualityComparer(IEqualityComparer<TKey1> equalityComparer1, IEqualityComparer<TKey2> equalityComparer2)
            {
                this.equalityComparer1 = equalityComparer1;
                this.equalityComparer2 = equalityComparer2;
            }

            /// <inheritdoc/>
            public bool Equals(KeyPack<TKey1, TKey2> x, KeyPack<TKey1, TKey2> y)
            {
                return this.equalityComparer1.Equals(x.key1, y.key1) && this.equalityComparer2.Equals(x.key2, y.key2);
            }

            /// <inheritdoc/>
            public int GetHashCode(KeyPack<TKey1, TKey2> obj)
            {
                return (this.equalityComparer1.GetHashCode(obj.key1) * 397) ^ this.equalityComparer2.GetHashCode(obj.key2);
            }
        }
    }
}
