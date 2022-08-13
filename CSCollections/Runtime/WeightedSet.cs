using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class WeightedSet<T> : ICollection<T>
    {
        private Random rand;

        public bool logWhileTaking = false;

        private float cachedWeightSum = -1f;

        private Dictionary<T, float> managedItems = new Dictionary<T, float>();

        public int Count => managedItems.Count;

        public bool IsReadOnly => false;

        public WeightedSet()
            : this(new Random())
        {
        }

        public WeightedSet(Random rand)
        {
            this.rand = rand;
        }

        public void AddOrUpdate(IDictionary<T, float> items)
        {
            foreach (var pair in items)
            {
                managedItems[pair.Key] = pair.Value;
            }

            cachedWeightSum = -1f;
        }

        public void Add(T item, float weight)
        {
            weight = Math.Max(weight, 0);
            managedItems.Add(item, weight);

            cachedWeightSum = -1f;
        }

        public void Update(T item, float weight)
        {
            weight = Math.Max(weight, 0);
            managedItems[item] = weight;

            cachedWeightSum = -1f;
        }

        public void Clear()
        {
            managedItems.Clear();
        }

        public bool Remove(T item)
        {
            return managedItems.Remove(item);
        }

        private IEnumerable<T> InternalRandomTake(int count)
        {
            if (count == 1)
            {
                yield return RandomTake();
                yield break;
            }

            if (cachedWeightSum < 0)
            {
                cachedWeightSum = managedItems.Sum(item => item.Value);
            }
            float weightSum = cachedWeightSum;

            // deepcopy
            List<KeyValuePair<T, float>> deepcopy = new List<KeyValuePair<T, float>>(managedItems);

            for (var i = 0; i < count; i++)
            {
                float ran = (float)rand.NextDouble() * weightSum;

                float sum = 0f;
                for (var j = 0; j < deepcopy.Count; j++)
                {
                    sum += deepcopy[j].Value;

                    if (logWhileTaking)
                    {
                        UnityEngine.Debug.Log($"index={j} sum={sum} ran={ran}");
                    }

                    if (sum > ran)
                    {
                        yield return deepcopy[j].Key;
                        weightSum -= deepcopy[j].Value;

                        // 移到最后一个
                        var last = deepcopy[deepcopy.Count - 1];
                        deepcopy[j] = last;
                        deepcopy.RemoveAt(deepcopy.Count - 1);

                        break;
                    }
                }
            }
        }

        public IEnumerable<T> RandomTake(int count)
        {
            if (count > managedItems.Count)
            {
                throw new Exception("not enough items");
            }

            if (count == managedItems.Count)
            {
                return managedItems.Select(e => e.Key);
            }

            return InternalRandomTake(count);
        }

        public T RandomTake()
        {
            if (managedItems.Count == 0)
            {
                throw new Exception("not enough items");
            }

            if (cachedWeightSum < 0)
            {
                cachedWeightSum = managedItems.Sum(item => item.Value);
            }
            float weightSum = cachedWeightSum;
            float ran = (float)rand.NextDouble() * weightSum;
            float sum = 0f;
            foreach (var pair in managedItems)
            {
                sum += pair.Value;

                if (logWhileTaking)
                {
                    UnityEngine.Debug.Log($"index={0} sum={sum} ran={ran}");
                }

                if (sum > ran)
                {
                    return pair.Key;
                }
            }

            throw new IndexOutOfRangeException();
        }

        public void Add(T item)
        {
            Add(item, 1);
        }

        public bool Contains(T item)
        {
            return managedItems.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return managedItems.Select(e => e.Key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
