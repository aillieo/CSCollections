// -----------------------------------------------------------------------
// <copyright file="WeightedSetTest.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [Category(nameof(WeightedSetTest))]
    public class WeightedSetTest
    {
        [Test]
        public static void TestHalf()
        {
            WeightedSet<int> set = new WeightedSet<int>();
            set.Add(0, 0.5f);
            set.Add(1, 0.5f);

            float result = (float)Enumerable.Range(1, 100000).Select(i => set.RandomTake()).Average();
            UnityEngine.Debug.Log($"result = {result}");
            Assert.LessOrEqual(Math.Abs(result - 0.5f), 0.01f);
        }

        [Test]
        public static void TestAvg1()
        {
            WeightedSet<int> set = new WeightedSet<int>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                set.Add(i, 1);
            }

            var results = Enumerable.Range(1, 100000).Select(i => set.RandomTake());
            var statisticInfo = StatisticHelper.GetStatisticInfo(results);
            UnityEngine.Debug.Log($"statisticInfo = {statisticInfo}");
        }

        [Test]
        public static void TestAvg2()
        {
            WeightedSet<int> set = new WeightedSet<int>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                set.Add(i, 1);
            }

            var results = Enumerable.Range(1, 100000).SelectMany(i => set.RandomTake(2));
            var statisticInfo = StatisticHelper.GetStatisticInfo(results);
            UnityEngine.Debug.Log($"statisticInfo = {statisticInfo}");
        }

        [Test]
        public static void TestAvg3()
        {
            WeightedSet<int> set = new WeightedSet<int>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                set.Add(i, i == 0 ? 2 : 1);
            }

            var results = Enumerable.Range(1, 100000).SelectMany(i => set.RandomTake(2));
            var statisticInfo = StatisticHelper.GetStatisticInfo(results);
            UnityEngine.Debug.Log($"statisticInfo = {statisticInfo}");
        }
    }
}
