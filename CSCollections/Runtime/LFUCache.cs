using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class LFUCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
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

        private static readonly float defaultShrinkFactor = 0.75f;
        private static readonly int defaultCapacity = 255;

        private readonly Dictionary<TKey, ValueWrapper> dictionary;
        private readonly LinkedDictionary<TKey, ValueWrapper>[] dictByFrequency;
        private readonly int maxFrequency;
        private readonly int capacity;
        private readonly float shrinkFactor;

        private int lowestFrequency;

        public LFUCache()
            : this(defaultCapacity, defaultShrinkFactor)
        {
        }

        public LFUCache(int capacity)
            : this(capacity, defaultShrinkFactor)
        {
        }

        public LFUCache(int capacity, float shrinkFactor)
            : this(capacity, null, shrinkFactor)
        {
        }

        public LFUCache(IEqualityComparer<TKey> comparer)
            : this(defaultCapacity, comparer, defaultShrinkFactor)
        {
        }

        public LFUCache(int capacity, IEqualityComparer<TKey> comparer)
            : this(capacity, comparer, defaultShrinkFactor)
        {
        }

        public LFUCache(int capacity, IEqualityComparer<TKey> comparer, float shrinkFactor)
        {
            if (shrinkFactor <= 0 || shrinkFactor >= 1)
            {
                throw new ArgumentOutOfRangeException($"invalid argument {nameof(shrinkFactor)}: expected range (0,1)");
            }

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException($"invalid argument {nameof(capacity)}");
            }

            dictionary = new Dictionary<TKey, ValueWrapper>(capacity);
            dictByFrequency = new LinkedDictionary<TKey, ValueWrapper>[capacity];
            lowestFrequency = 0;
            maxFrequency = capacity - 1;
            this.capacity = capacity;
            this.shrinkFactor = shrinkFactor;
        }

        public void Add(TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added");
            }

            if (dictionary.Count == capacity)
            {
                Shrink();
            }

            LinkedDictionary<TKey, ValueWrapper> dict = GetDictByFrequency(0);
            ValueWrapper wrapper = new ValueWrapper(value, 0);
            dict.Add(key, wrapper);
            dictionary[key] = wrapper;
            lowestFrequency = 0;
        }

        public bool Remove(TKey key)
        {
            ValueWrapper wrapper = default;
            if (dictionary.TryGetValue(key, out wrapper))
            {
                LinkedDictionary<TKey, ValueWrapper> dict = GetDictByFrequency(wrapper.frequency);
                dict.Remove(key);
                if (lowestFrequency == wrapper.frequency)
                {
                    UpdateLowestFrequency();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            for (int i = 0; i <= maxFrequency; i++)
            {
                if (dictByFrequency[i] != null)
                {
                    dictByFrequency[i].Clear();
                }
            }
            dictionary.Clear();
            lowestFrequency = 0;
        }

        public int Count => dictionary.Count;
        public int Capacity => capacity;
        public ICollection<TKey> Keys => this.Select(pair => pair.Key).ToList().AsReadOnly();

        public ICollection<TValue> Values => this.Select(pair => pair.Value).ToList().AsReadOnly();

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                TValue value = default;
                if (TryGetValue(key, out value))
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
                if (!dictionary.TryGetValue(key, out wrapper))
                {
                    Add(key, value);
                }
                else
                {
                    wrapper.value = value;
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        private LinkedDictionary<TKey, ValueWrapper> GetDictByFrequency(int frequency)
        {
            if (frequency < 0 || frequency > maxFrequency)
            {
                throw new Exception();
            }

            if (dictByFrequency[frequency] == null)
            {
                dictByFrequency[frequency] = new LinkedDictionary<TKey, ValueWrapper>();
            }
            return dictByFrequency[frequency];
        }

        private void Shrink()
        {
            int removed = 0;
            int toRemove = dictionary.Count - (int)(shrinkFactor * capacity);
            while (removed < toRemove)
            {
                LinkedDictionary<TKey, ValueWrapper> dict = GetDictByFrequency(lowestFrequency);
                if (dict.Count == 0)
                {
                    throw new Exception();
                }

                while (dict.Count > 0 && removed < toRemove)
                {
                    var key = dict.FirstKey;

                    bool r = dict.Remove(key);

                    dictionary.Remove(key);

                    ++removed;
                }

                if (dict.Count == 0)
                {
                    UpdateLowestFrequency();
                }
            }
        }

        private void UpdateLowestFrequency()
        {
            while (lowestFrequency <= maxFrequency && GetDictByFrequency(lowestFrequency).Count == 0)
            {
                lowestFrequency++;
            }
            if (lowestFrequency > maxFrequency)
            {
                lowestFrequency = 0;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            ValueWrapper wrapper = default;
            if (dictionary.TryGetValue(key, out wrapper))
            {
                int currentFrequency = wrapper.frequency;
                if (currentFrequency < maxFrequency)
                {
                    int nextFrequency = currentFrequency + 1;

                    LinkedDictionary<TKey, ValueWrapper> curDict = GetDictByFrequency(currentFrequency);
                    LinkedDictionary<TKey, ValueWrapper> nextDict = GetDictByFrequency(nextFrequency);

                    curDict.Remove(key);
                    nextDict.Add(key, wrapper);
                    wrapper.frequency = nextFrequency;

                    dictionary[key] = wrapper;
                    if (lowestFrequency == currentFrequency && curDict.Count == 0)
                    {
                        lowestFrequency = nextFrequency;
                    }
                }
                else
                {
                    // f相同时 用LRU策略
                    LinkedDictionary<TKey, ValueWrapper> dict = GetDictByFrequency(currentFrequency);
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

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
