using System;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections.Tests
{
    public static class StatisticHelper
    {
        public readonly struct StatisticInfo
        {
            public readonly int count;
            public readonly int max;
            public readonly int min;
            public readonly double average;
            public readonly double variance;
            public readonly Dictionary<int, int> times;

            public StatisticInfo(int count, int max, int min, double a, double v, Dictionary<int, int> times)
            {
                this.count = count;
                this.max = max;
                this.min = min;
                this.average = a;
                this.variance = v;
                this.times = times;
            }

            public override string ToString()
            {
                int cnt = count;
                string timesStr = string.Join("\n", times.OrderBy(pair => pair.Value).Select(pair => $"{pair.Key}: {pair.Value}({(float)pair.Value / cnt})"));
                return $"count={count}\nmax={max}\nmin={min}\navg={average}\nvar={variance}\ntimes:\n{timesStr}";
            }
        }

        public static StatisticInfo GetStatisticInfo(IEnumerable<int> data)
        {
            Dictionary<int, int> times = new Dictionary<int, int>();
            foreach (var f in data)
            {
                if (times.TryGetValue(f, out int cnt))
                {
                    times[f] = cnt + 1;
                }
                else
                {
                    times.Add(f, 1);
                }
            }

            int max = int.MinValue;
            int min = int.MaxValue;
            double sum = 0;
            int count = 0;
            foreach (var f in data)
            {
                max = Math.Max(f, max);
                min = Math.Min(f, min);
                double d = (double)f;
                sum += d;
                count++;
            }

            double avg = sum / count;

            double sqSum = 0;
            foreach (var f in data)
            {
                double dt = (double)f - avg;
                sqSum += (dt * dt);
            }

            double v = sqSum / (count - 1);
            return new StatisticInfo(count, max, min, avg, v, times);
        }
    }
}
