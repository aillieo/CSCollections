// -----------------------------------------------------------------------
// <copyright file="PriorityQueueTest`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections.Tests
{
    using NUnit.Framework;

    [Category(nameof(PriorityQueueTest_1))]
    public class PriorityQueueTest_1
    {
        [Test]
        public static void Test()
        {
            PriorityQueue<int> queue = new PriorityQueue<int>();
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(0);
            queue.Enqueue(1);
            Assert.AreEqual(queue.Count, 4);
            Assert.AreEqual(queue.Dequeue(), 3);
            Assert.AreEqual(queue.Dequeue(), 2);
            Assert.AreEqual(queue.Dequeue(), 1);
            Assert.AreEqual(queue.Dequeue(), 0);
            Assert.AreEqual(queue.Count, 0);
        }
    }
}
