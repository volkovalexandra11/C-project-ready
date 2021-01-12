//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.ExceptionServices;

//namespace TokenParsing.Infrastructure
//{
//    public class PriorityPartitioner
//    {
//        private string StringToPartition { get; }
//        private string[] PossibleParts { get; }

//        public Partition<string> Partition { get; }

//        public PriorityPartitioner(string stringToPartition, string[] possiblePartsOrdered)
//        {
//            StringToPartition = stringToPartition;
//            PossibleParts = possiblePartsOrdered;
//            Partition = new Partition<string>(StringToPartition.Length);
//            MakePartition();
//        }

//        public string[] GetPartitionResult(bool withEmptyParts)
//        {
//            var parts = Partition.FilledParts.AsEnumerable();

//            if (withEmptyParts)
//            {
//                parts = parts
//                    .Concat(Partition.EmptyParts)
//                    .OrderBy(part => part.Start);
//            }

//            return parts
//                .Select(part => StringToPartition.Substring(part.Start, part.Length))
//                .ToArray();
//        }

//        private void MakePartition()
//        {
//            foreach (var part in PossibleParts)
//            {
//                FindParts(part);
//            }
//        }

//        private void FindParts(string part)
//        {
//            var emptyPartsBeforeIterating = Partition.EmptyParts.ToArray();
//            foreach (var emptyPart in emptyPartsBeforeIterating)
//            {
//                FindPartOnSlice(part, emptyPart.Start, emptyPart.Start + emptyPart.Length);
//            }
//        }

//        private void FindPartOnSlice(string part, int start, int end)
//        {
//            var indices = StringToPartition.NoOverlapIndexesOf(part, start, end);
//            foreach (var index in indices)
//            {
//                Partition.AddNewPart(part, index, part.Length);
//            }
//        }
//    }
//}