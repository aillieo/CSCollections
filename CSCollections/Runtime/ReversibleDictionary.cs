using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
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

        public bool HasKeyForValue(TValue value)
        {
            if (lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                return keys.Count > 0;
            }

            return false;
        }

        public TKey FirstKeyForValue(TValue value)
        {
            if (lookup.TryGetValue(value, out HashSet<TKey> keys))
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
            if (lookup.TryGetValue(value, out HashSet<TKey> keys))
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
            if (lookup.TryGetValue(value, out HashSet<TKey> keys))
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
            int count = 0;
            if (lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                foreach (var k in keys)
                {
                    dictionary.Remove(k);
                    count++;
                }

                lookup.Remove(value);
            }

            return count;
        }

        public void ClearLight()
        {
            dictionary.Clear();
            foreach (var pair in lookup)
            {
                pair.Value.Clear();
            }
        }

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set
            {
                if (dictionary.TryGetValue(key, out TValue v))
                {
                    // replace
                    InternalRemoveKeyForValue(v, key);
                    dictionary[key] = value;
                    InternalAddKeyForValue(value, key);
                }
                else
                {
                    // add
                    dictionary.Add(key, value);
                    InternalAddKeyForValue(value, key);
                }
            }
        }

        public ICollection<TKey> Keys => dictionary.Keys;

        public ICollection<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            InternalAddKeyForValue(value, key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item.Key, item.Value);
            InternalAddKeyForValue(item.Value, item.Key);
        }

        public void Clear()
        {
            dictionary.Clear();
            lookup.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                dictionary.Remove(key);
                InternalRemoveKeyForValue(value, key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item))
            {
                InternalRemoveKeyForValue(item.Value, item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void InternalAddKeyForValue(TValue value, TKey key)
        {
            if (!lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                keys = new HashSet<TKey>();
                lookup.Add(value, keys);
            }

            keys.Add(key);
        }

        private void InternalRemoveKeyForValue(TValue value, TKey key)
        {
            if (!lookup.TryGetValue(value, out HashSet<TKey> keys))
            {
                return;
            }

            keys.Remove(key);
        }
    }
}
