// -----------------------------------------------------------------------
// <copyright file="FIFOCacheTest.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections.Tests
{
    using NUnit.Framework;

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
