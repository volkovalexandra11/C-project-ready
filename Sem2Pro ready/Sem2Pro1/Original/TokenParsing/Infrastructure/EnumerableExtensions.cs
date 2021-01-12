using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TokenParsing.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static int[] GetMinIndexes<T>(this IEnumerable<T> source)
            where T : IComparable<T>
        {
            var sourceArr = source.ToArray();
            var min = sourceArr.Min();
            return Enumerable.Range(0, sourceArr.Length)
                .Where(ind => sourceArr[ind].CompareTo(min) == 0)
                .ToArray();
        }

        public static bool IsOrdered<T>(this IEnumerable<T> source, bool ascending)
            where T : IComparable<T>
        {
            var firstFound = false;
            var prevEl = default(T);

            foreach (var el in source)
            {
                if (!firstFound)
                {
                    firstFound = true;
                    prevEl = el;
                    continue;
                }

                if (ascending && prevEl.CompareTo(el) > 0
                    || !ascending && prevEl.CompareTo(el) < 0)
                {
                    return false;
                }

                prevEl = el;
            }
            return true;
        }

        public static bool HasConsecutiveRepeats<T>(this IEnumerable<T> source)
            where T : IComparable<T>
        {
            var foundFirst = false;
            var prevEl = default(T);

            foreach (var el in source)
            {
                if (!foundFirst)
                {
                    foundFirst = true;
                    prevEl = el;
                    continue;
                }

                if (el.CompareTo(prevEl) == 0) return true;

                prevEl = el;
            }
            return false;
        }
    }
}