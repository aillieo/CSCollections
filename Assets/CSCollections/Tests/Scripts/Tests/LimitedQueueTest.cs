// -----------------------------------------------------------------------
// <copyright file="LimitedQueueTest.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections.Tests
{
    using NUnit.Framework;

    [Category(nameof(LimitedQueueTest))]
    public class LimitedQueueTest
    {
        [Test]
        public static void TestEnqueue()
        {
            LimitedQueue<int> queue = new LimitedQueue<int>(3);
            queue.Add(1);
            queue.Add(2);
            queue.Add(3);
            Assert.AreEqual(queue.Count, 3);
            queue.Add(4);
            Assert.AreEqual(queue.Count, 3);
        }
    }
}
