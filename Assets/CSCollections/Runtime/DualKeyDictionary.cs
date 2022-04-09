using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public struct KeyPack<TKey1, TKey2>
    {
        public TKey1 key1;
        public TKey2 key2;

        public KeyPack(TKey1 key1, TKey2 key2)
        {
            this.key1 = key1;
            this.key2 = key2;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class DualKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>, IDictionary<KeyPack<TKey1, TKey2>, TValue>
    {
        private Dictionary<KeyPack<TKey1, TKey2>, TValue> dict;

        public DualKeyDictionary()
        {
            dict = new Dictionary<KeyPack<TKey1, TKey2>, TValue>();
        }

        public DualKeyDictionary(int capacity)
        {
            throw new NotImplementedException();
        }

        public DualKeyDictionary(int capacity, IEqualityComparer<TKey1> key1Comparer, IEqualityComparer<TKey2> key2Comparer)
        {
            throw new NotImplementedException();
        }

        public DualKeyDictionary(int capacity, IEqualityComparer<KeyPack<TKey1, TKey2>> comparer)
        {
            throw new NotImplementedException();
        }

        public DualKeyDictionary(IEqualityComparer<TKey1> key1Comparer, IEqualityComparer<TKey2> key2Comparer)
        {
            throw new NotImplementedException();
        }

        public DualKeyDictionary(IEqualityComparer<KeyPack<TKey1, TKey2>> comparer)
        {
            throw new NotImplementedException();
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

        public bool IsReadOnly => ((ICollection<KeyValuePair<KeyPack<TKey1, TKey2>, TValue>>)dict).IsReadOnly;

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
            throw new System.NotImplementedException();
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
