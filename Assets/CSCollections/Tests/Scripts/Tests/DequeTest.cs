// -----------------------------------------------------------------------
// <copyright file="DequeTest.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections.Tests
{
    using NUnit.Framework;

    [Category(nameof(DequeTest))]
    public class DequeTest
    {
        [Test]
        public static void TestPushLeft()
        {
            Deque<int> deque = new Deque<int>();
            deque.PushLeft(1);
            Assert.AreEqual(deque.Count, 1);
            Assert.AreEqual(deque.PeekLeft(), 1);
            Assert.AreEqual(deque.PeekRight(), 1);
            Assert.AreEqual(deque.PopLeft(), 1);
            Assert.AreEqual(deque.Count, 0);
        }

        [Test]
        public static void TestPushRight()
        {
            Deque<int> deque = new Deque<int>();
            deque.PushRight(1);
            Assert.AreEqual(deque.Count, 1);
            Assert.AreEqual(deque.PeekLeft(), 1);
            Assert.AreEqual(deque.PeekRight(), 1);
            Assert.AreEqual(deque.PopRight(), 1);
            Assert.AreEqual(deque.Count, 0);
        }
    }
}
