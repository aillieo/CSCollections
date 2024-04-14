// -----------------------------------------------------------------------
// <copyright file="ReversibleDictionaryTest.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections.Tests
{
    using NUnit.Framework;

    [Category(nameof(ReversibleDictionaryTest))]
    public class ReversibleDictionaryTest
    {
        [Test]
        public static void TestAdd()
        {
            ReversibleDictionary<int, int> dictionary = new ReversibleDictionary<int, int>();
            dictionary.Add(1, 1);
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(dictionary.FirstKeyForValue(1), 1);
            Assert.AreEqual(dictionary.FirstKeyForValueOrDefault(4, 0), 0);
        }
    }
}
