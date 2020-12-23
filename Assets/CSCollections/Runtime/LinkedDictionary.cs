using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
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

        private int version;
        private readonly Dictionary<TKey, ValueWrapper> dict;

        private TKey firstKey;

        private TKey lastKey;

        public TKey FirstKey
        {
            get
            {
                if (dict.Count == 0)
                {
                    throw new KeyNotFoundException(string.Empty);
                }

                return firstKey;
            }
        }

        public TKey LastKey
        {
            get
            {
                if (dict.Count == 0)
                {
                    throw new KeyNotFoundException(string.Empty);
                }

                return lastKey;
            }
        }

        public ICollection<TKey> Keys => this.Select(pair => pair.Key).ToList().AsReadOnly();

        public ICollection<TValue> Values => this.Select(pair => pair.Value).ToList().AsReadOnly();

        public int Count => dict.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => dict[key].value;
            set
            {
                if (dict.TryGetValue(key, out ValueWrapper wrapper))
                {
                    wrapper.value = value;
                    version++;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public LinkedDictionary() : this(0)
        { }

        public LinkedDictionary(IEqualityComparer<TKey> comparer) : this(0, comparer)
        { }

        public LinkedDictionary(int capacity)
        {
            dict = new Dictionary<TKey, ValueWrapper>(capacity);
        }

        public LinkedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            dict = new Dictionary<TKey, ValueWrapper>(capacity, comparer);
        }

        public void AddLast(TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            ValueWrapper wrapper = ValueWrapper.Wrap(value);
            if (dict.Count == 0)
            {
                firstKey = key;
                lastKey = key;
                wrapper.nextKey = key;
                wrapper.previousKey = key;
            }
            else
            {
                ValueWrapper first = dict[firstKey];
                ValueWrapper last = dict[lastKey];
                first.previousKey = key;
                last.nextKey = key;
                wrapper.previousKey = lastKey;
                wrapper.nextKey = firstKey;
                lastKey = key;
            }

            InternalAdd(key, wrapper);
        }

        public void AddFirst(TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            ValueWrapper wrapper = ValueWrapper.Wrap(value);
            if (dict.Count == 0)
            {
                firstKey = key;
                lastKey = key;
                wrapper.nextKey = key;
                wrapper.previousKey = key;
            }
            else
            {
                ValueWrapper first = dict[firstKey];
                ValueWrapper last = dict[lastKey];
                first.previousKey = key;
                last.nextKey = key;
                wrapper.previousKey = lastKey;
                wrapper.nextKey = firstKey;
                firstKey = key;
            }

            InternalAdd(key, wrapper);
        }

        public void Add(TKey key, TValue value)
        {
            AddLast(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            if (dict.TryGetValue(key, out ValueWrapper wrapper))
            {
                return InternalRemove(key, wrapper);
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            if (dict.TryGetValue(key, out ValueWrapper wrapper))
            {
                value = wrapper.value;
                return true;
            }

            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            foreach (var pair in dict)
            {
                ValueWrapper.Recycle(pair.Value);
            }

            firstKey = lastKey = default;
            dict.Clear();
            version++;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (dict.TryGetValue(item.Key, out ValueWrapper wrapper))
            {
                return EqualityComparer<TValue>.Default.Equals(wrapper.value, item.Value);
            }

            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException($"invalid argument {nameof(array)}");
            }

            if (arrayIndex < 0 || array.Length + arrayIndex < dict.Count)
            {
                throw new ArgumentOutOfRangeException($"invalid argument {nameof(arrayIndex)}");
            }

            var data = dict.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.value)).ToArray();
            Array.Copy(data, 0, array, arrayIndex, data.Length);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (dict.TryGetValue(item.Key, out ValueWrapper wrapper) &&
                EqualityComparer<TValue>.Default.Equals(wrapper.value, item.Value))
            {
                return InternalRemove(item.Key, wrapper);
            }

            return false;
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            int v = version;
            if (dict.Count > 0)
            {
                TKey key = firstKey;
                while (true)
                {
                    if (version != v)
                    {
                        throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                    }

                    ValueWrapper wrapper = dict[key];
                    yield return new KeyValuePair<TKey, TValue>(key, wrapper.value);
                    key = wrapper.nextKey;

                    if(dict.Comparer.Equals(key, firstKey))
                    {
                        break;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
        }

        private void InternalAdd(TKey key, ValueWrapper wrapper)
        {
            dict.Add(key, wrapper);
            version++;
        }

        private bool InternalRemove(TKey key, ValueWrapper wrapper)
        {
            if (dict.Count == 1)
            {
                firstKey = default;
                lastKey = default;
            }
            else
            {
                if (dict.TryGetValue(wrapper.nextKey, out ValueWrapper next))
                {
                    next.previousKey = wrapper.previousKey;
                }

                if (dict.TryGetValue(wrapper.previousKey, out ValueWrapper previous))
                {
                    previous.nextKey = wrapper.nextKey;
                }

                if (dict.Comparer.Equals(firstKey, key))
                {
                    firstKey = wrapper.nextKey;
                }

                if (dict.Comparer.Equals(lastKey, key))
                {
                    lastKey = wrapper.previousKey;
                }
            }

            ValueWrapper.Recycle(wrapper);
            dict.Remove(key);

            version++;
            return true;
        }
    }
}
