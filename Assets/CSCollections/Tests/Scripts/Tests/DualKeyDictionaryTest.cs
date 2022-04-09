using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(DualKeyDictionaryTest))]
    public class DualKeyDictionaryTest
    {
        [Test]
        public static void TestAdd()
        {
            DualKeyDictionary<int, int, string> dict = new DualKeyDictionary<int, int, string>();
            dict.Add(0, 0, "0,0");
            dict.Add(0, 1, "0,1");
            dict.Add(1, 0, "1,0");
            dict.Add(1, 1, "1,1");

            Assert.AreEqual(dict.Count, 4);
            Assert.IsTrue(dict.ContainsKey(1, 1));
            Assert.AreEqual(dict[1, 1], "1,1");
        }
    }
}
