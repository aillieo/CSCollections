using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public struct KeyPack<TKey1, TKey2> : IEquatable<KeyPack<TKey1, TKey2>>
    {
        public readonly TKey1 key1;
        public readonly TKey2 key2;

        public KeyPack(TKey1 key1, TKey2 key2)
        {
            this.key1 = key1;
            this.key2 = key2;
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyPack<TKey1, TKey2> key)
            {
                return this.Equals(key);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return key1.GetHashCode() * 397 ^ key2.GetHashCode();
        }

        public bool Equals(KeyPack<TKey1, TKey2> other)
        {
            return EqualityComparer<TKey1>.Default.Equals(key1, other.key1)
                   && EqualityComparer<TKey2>.Default.Equals(key2, other.key2);
        }
    }

    public class DualKeyDictionary<TKey1, TKey2, TValue> : IDictionary<KeyPack<TKey1, TKey2>, TValue>
    {
        public class EqualityComparer : IEqualityComparer<KeyPack<TKey1, TKey2>>
        {
            private readonly IEqualityComparer<TKey1> equalityComparer1;
            private readonly IEqualityComparer<TKey2> equalityComparer2;

            public EqualityComparer(IEqualityComparer<TKey1> equalityComparer1, IEqualityComparer<TKey2> equalityComparer2)
            {
                this.equalityComparer1 = equalityComparer1;
                this.equalityComparer2 = equalityComparer2;
            }

            public bool Equals(KeyPack<TKey1, TKey2> x, KeyPack<TKey1, TKey2> y)
            {
                return this.equalityComparer1.Equals(x.key1, y.key1) && this.equalityComparer2.Equals(x.key2, y.key2);
            }

            public int GetHashCode(KeyPack<TKey1, TKey2> obj)
            {
                return (equalityComparer1.GetHashCode(obj.key1) * 397) ^ equalityComparer2.GetHashCode(obj.key2);
            }
        }

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
            dict = new Dictionary<KeyPack<TKey1, TKey2>, TValue>(capacity, comparer);
        }

        public DualKeyDictionary(IEqualityComparer<TKey1> key1Comparer, IEqualityComparer<TKey2> key2Comparer)
            : this(0, new EqualityComparer(key1Comparer, key2Comparer))
        {
        }

        public DualKeyDictionary(IEqualityComparer<KeyPack<TKey1, TKey2>> comparer)
            : this(0, comparer)
        {
        }

        public TValue this[KeyPack<TKey1, TKey2> key]
        {
            get => dict[key];
            set => dict[key] = value;
        }

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get => dict[new KeyPack<TKey1, TKey2>(key1, key2)];
            set => dict[new KeyPack<TKey1, TKey2>(key1, key2)] = value;
        }

        public ICollection<KeyPack<TKey1, TKey2>> Keys => dict.Keys;

        public ICollection<TKey1> AllKey1 => dict.Select(kvp => kvp.Key.key1).Distinct().ToArray();

        public ICollection<TKey2> AllKey2 => dict.Select(kvp => kvp.Key.key2).Distinct().ToArray();

        public ICollection<TValue> Values => dict.Values;

        public int Count => dict.Count;

        bool ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>.IsReadOnly => false;

        public void Add(KeyPack<TKey1, TKey2> key, TValue value)
        {
            dict.Add(key, value);
        }

        public void Add(KeyValuePair<KeyPack<TKey1, TKey2>, TValue> item)
        {
            ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)dict).Add(item);
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            dict.Add(new KeyPack<TKey1, TKey2>(key1, key2), value);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(KeyValuePair<KeyPack<TKey1, TKey2>, TValue> item)
        {
            return ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)dict).Contains(item);
        }

        public bool ContainsKey(KeyPack<TKey1, TKey2> key)
        {
            return dict.ContainsKey(key);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            return dict.ContainsKey(new KeyPack<TKey1, TKey2>(key1, key2));
        }

        public bool ContainsKey1(TKey1 key1)
        {
            return dict.Any(kvp => kvp.Key.key1.Equals(key1));
        }

        public bool ContainsKey2(TKey2 key2)
        {
            return dict.Any(kvp => kvp.Key.key2.Equals(key2));
        }

        public void CopyTo(KeyValuePair<KeyPack<TKey1, TKey2>, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)dict).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        public bool Remove(KeyPack<TKey1, TKey2> key)
        {
            return dict.Remove(key);
        }

        public bool Remove(TKey1 key1, TKey2 key2)
        {
            return dict.Remove(new KeyPack<TKey1, TKey2>(key1, key2));
        }

        public bool Remove(KeyValuePair<KeyPack<TKey1, TKey2>, TValue> item)
        {
            return ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)dict).Remove(item);
        }

        public bool TryGetValue(KeyPack<TKey1, TKey2> key, out TValue value)
        {
            return dict.TryGetValue(key, out value);
        }

        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
        {
            return dict.TryGetValue(new KeyPack<TKey1, TKey2>(key1, key2), out value);
        }

        public IEnumerable<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>> EnumKey1(TKey1 key1)
        {
            return dict.Where(kvp => kvp.Key.key1.Equals(key1));
        }

        public IEnumerable<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>> EnumKey2(TKey2 key2)
        {
            return dict.Where(kvp => kvp.Key.key2.Equals(key2));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
