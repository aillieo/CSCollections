using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace AillieoUtils.Collections.Tests
{
    public static class Utils
    {
        public static void Print<T>(IEnumerable<T> enumerable)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var e in enumerable)
            {
                sb.AppendLine($"{e}");
            }

            sb.AppendLine("}");
            UnityEngine.Debug.Log(sb.ToString());
        }

        public static void Print<T, U>(IEnumerable<KeyValuePair<T, U>> enumerable)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var e in enumerable)
            {
                sb.AppendLine($"{e.Key}={e.Value}");
            }

            sb.AppendLine("}");
            UnityEngine.Debug.Log(sb.ToString());
        }
    }
}
