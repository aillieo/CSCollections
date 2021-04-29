using UnityEngine;

namespace AillieoUtils.Collections.Tests
{
    public class CollectionsTestRunner : MonoBehaviour
    {
        private void Start()
        {
            LinkedDictionaryTest.Test();
            LRUCacheTest.TestLRU();
            LFUCacheTest.TestLFU();
            FIFOCacheTest.Test();
            DequeTest.Test();
            LimitedQueueTest.Test();
            UniqueQueueTest.Test();
            PriorityQueueTest.Test();
            UniquePriorityQueueTest.Test();
            WeightedSetTest.Test();
            Debug.Log("pass all");
        }
    }
}
