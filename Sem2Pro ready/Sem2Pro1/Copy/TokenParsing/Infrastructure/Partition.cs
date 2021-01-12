//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;

//namespace TokenParsing.Infrastructure
//{
//    public class Partition<TId> : IEnumerable<Partition<TId>.Part>
//        where TId : IComparable<TId>
//    {
//        public class Part
//        {
//            public readonly TId Id;
//            public readonly int Start;
//            public readonly int Length;

//            public Part(int start, int length)
//                : this(default, start, length)
//            {
//            }

//            public Part(TId id, int start, int length)
//            {
//                Id = id;
//                Start = start;
//                Length = length;
//            }
//        }

//        public class PartStartComparer : IComparer<Part>
//        {
//            public int Compare(Part left, Part right)
//            {
//                if (left is null || right is null)
//                    throw new ArgumentNullException();
//                return left.Start.CompareTo(right.Start);
//            }
//        }

//        public int Length { get; }
//        public IReadOnlyCollection<Part> EmptyParts => emptyParts;
//        public IReadOnlyCollection<Part> FilledParts => filledParts;

//        private Part[] PartsAtPos { get; }
//        private bool[] Assigned { get; }

//        private readonly SortedSet<Part> emptyParts;
//        private readonly SortedSet<Part> filledParts;

//        public Partition(int length)
//        {
//            Length = length;
//            Assigned = new bool[length];

//            var emptyPart = new Part(0, length);
//            PartsAtPos = Enumerable.Repeat(emptyPart, length).ToArray();

//            emptyParts = new SortedSet<Part>(new PartStartComparer())
//            {
//                new Part(0, length)
//            };

//            filledParts = new SortedSet<Part>(new PartStartComparer());
//        }

//        public void AddNewPart(TId id, int startInd, int partLength)
//        {
//            if (id is null)
//                throw new ArgumentNullException($"{nameof(id)}");
//            if (partLength <= 0)
//                throw new ArgumentException($"Part length should be a positive integer, got {partLength}");

//            var newPart = new Part(id, startInd, partLength);
//            filledParts.Add(newPart);

//            if (Assigned[startInd..(startInd + partLength)].Any(isAssigned => isAssigned))
//                throw new InvalidOperationException($"Element already in a non-empty part found");

//            var emptyPartToSplit = PartsAtPos[startInd];

//            SplitEmptyPartBetween(emptyPartToSplit, startInd, startInd + partLength);

//            Array.Fill(PartsAtPos, newPart, startInd, partLength);
//            Array.Fill(Assigned, true, startInd, partLength);
//        }

//        public IEnumerator<Part> GetEnumerator()
//        {
//            return filledParts.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//        private void SplitEmptyPartBetween(Part emptyPart, int startInd, int endInd)
//        {
//            var emptyPartEnd = emptyPart.Start + emptyPart.Length;
//            emptyParts.Remove(emptyPart);

//            if (emptyPart.Start < startInd)
//            {
//                AddEmptyPartBetween(emptyPart.Start, startInd);
//            }
//            if (emptyPartEnd > endInd)
//            {
//                AddEmptyPartBetween(endInd, emptyPartEnd);
//            }
//        }

//        private void AddEmptyPartBetween(int startPos, int endPos)
//        {
//            var newEmptyPart = new Part(startPos, endPos - startPos);
//            emptyParts.Add(newEmptyPart);
//            Array.Fill(PartsAtPos, newEmptyPart, startPos, endPos - startPos);
//        }
//    }
//}