using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(FIFOCacheTest))]
    public class FIFOCacheTest
    {
        [Test]
        public static void TestReplace()
        {
            FIFOCache<int, int> cache = new FIFOCache<int, int>(2);
            cache[1] = 1;
            cache[2] = 2;
            Assert.AreEqual(cache.Count, 2);
            cache[3] = 3;
            Assert.IsFalse(cache.ContainsKey(1));
            Assert.IsTrue(cache.ContainsKey(3));
        }
    }
}
