using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Infrastructure.Infrastructure;

namespace Infrastructure.TopDowns
{
    public class PrioritizedString
    {
        public string Input { get; }
        public int[] Priorities { get; }

        public PrioritizedString(string input)
        {
            Input = input;
            Priorities = GetPriorities(input);
        }

        private PrioritizedString(PrioritizedString fullString, int startInd, int length)
        {
            Input = fullString.Input.Substring(startInd, length);

            Priorities = fullString.Priorities.Skip(startInd).Take(length).ToArray();
        }

        public PrioritizedString Substring(int startInd, int length)
        {
            return new PrioritizedString(this, startInd, length);
        }

        public PrioritizedString Trim()
        {
            var trimmed = Input.Trim();
            var startInd = Input.IndexOf(trimmed);
            return new PrioritizedString(this, startInd, trimmed.Length);
        }

        public PrioritizedString[] SplitOnSubset(string splitter, int[] subsetIndexes)
        {
            var splitString = Input.SplitOnSubset(splitter, subsetIndexes);
            var parts = new List<PrioritizedString>();

            var lastEndInd = -splitter.Length;
            foreach (var part in splitString)
            {
                var partStartInd = Input.IndexOf(part, lastEndInd + splitter.Length);
                parts.Add(new PrioritizedString(this, partStartInd, part.Length));
                lastEndInd = partStartInd + part.Length;
            }

            return parts.ToArray();
        }

        private static int[] GetPriorities(string input)
        {
            var priorities = new int[input.Length];
            var currPriority = 0;

            for (var ind = 0; ind < input.Length; ind++)
            {
                var c = input[ind];

                if (c.IsOpeningBracket())
                {
                    currPriority += (int)BasePriority.BracketDelta;
                }
                else if (c.IsClosingBracket())
                {
                    currPriority -= (int)BasePriority.BracketDelta;
                }

                priorities[ind] = currPriority;
            }

            return priorities;
        }
    }
}