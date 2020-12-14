using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Infrastructure
{
    public static class StringExtensions
    {
        public static string[] SplitOnSubset(this string s, string splitter, int[] subsetIndexes)
        {
            if (!AreSubsetIndexes(s.Length, subsetIndexes))
            {
                throw new ArgumentException(
                    $"SubsetIndexes should be an ascending sequence of indexes of {nameof(s)}");
            }

            var parts = new List<string>();
            var prevEndInd = 0;
            var currSubsetIndInd = 0;
            while (currSubsetIndInd < subsetIndexes.Length)
            {
                var currSubsetInd = subsetIndexes[currSubsetIndInd];
                if (!SubstringStartsFromAndIsFromSubset(s, splitter, subsetIndexes, currSubsetInd))
                {
                    currSubsetIndInd++;
                    continue;
                }

                parts.Add(s.Substring(prevEndInd, currSubsetInd - prevEndInd));
                prevEndInd = currSubsetInd + splitter.Length;
                while (subsetIndexes[currSubsetIndInd] < prevEndInd)
                    currSubsetIndInd++;
            }
            parts.Add(s.Substring(prevEndInd, s.Length - prevEndInd));

            return parts.ToArray();
        }

        private static bool AreSubsetIndexes(int sourceLen, int[] subsetIndexes)
        {
            if (!subsetIndexes.IsOrdered(ascending: true)) return false;
            if (subsetIndexes.HasConsecutiveRepeats()) return false;
            if (subsetIndexes.First() < 0 || subsetIndexes.Last() >= sourceLen) return false;

            return true;
        }

        private static bool SubstringStartsFromAndIsFromSubset(
            string s, string substring, int[] subsetInds, int startPos)
        {
            if (!SubstringStartsFrom(s, substring, startPos)) return false;

            var subsetStartInd = Array.BinarySearch(subsetInds, startPos);
            if (subsetStartInd < 0) return false;
            if (subsetStartInd + substring.Length >= subsetInds.Length) return false;

            return Enumerable.Range(0, substring.Length)
                .All(substrInd => subsetInds[substrInd + subsetStartInd] == startPos + substrInd);
        }

        private static bool SubstringStartsFrom(string s, string substring, int startInd)
        {
            if (s.Length < startInd + substring.Length) return false;

            return !substring.Where((subChar, subInd) => s[startInd + subInd] != subChar).Any();
        }

        public static bool EndsWith(this string source, string value, int endInd)
        {
            return source.IndexOf(value, endInd - value.Length + 1, value.Length) > 0;
        }

        public static IEnumerable<int> NoOverlapIndexesOf(
            this string source, string value,
            int startInd, int endInd)
        {
            var currInd = startInd;
            var maxInd = endInd - value.Length;

            while (true)
            {
                currInd = source.IndexOf(value, currInd);

                if (0 <= currInd && currInd < maxInd)
                {
                    yield return currInd;
                    currInd += value.Length;
                }
                else
                    break;

            }
        }
    }
}