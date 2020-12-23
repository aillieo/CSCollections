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
            Debug.Log("pass all");
        }
    }
}
