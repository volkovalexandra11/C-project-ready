//using System;
//using System.Linq;
//using NUnit.Framework;
//using TokenParsing.Infrastructure;

//namespace Infrastructure_Tests
//{
//    public class Tests
//    {
//        [Test]
//        public void Test1()
//        {
//            var partition = new Partition<string>(2);
//            partition.AddNewPart("a", 0, 1);
//            Console.WriteLine(partition.EmptyParts.Count);
//        }

//        private static readonly object[] splitEmptyPartsSource
//            = 
//            {
//                new object[] { 5, new[] { (1, 2) }, new[] { (0, 1), (3, 2) } },
//                new object[] { 2, new[] { (0, 1) }, new[] { (1, 1) } },
//                new object[] { 10, new[] { (7, 3), (0, 2), (5, 1) }, new[] { (2, 3), (6, 1) } }
//            };

//        [TestCaseSource(nameof(splitEmptyPartsSource))]
//        public void SplitEmptyParts(
//            int length,
//            (int partStart, int partLength)[] parts,
//            (int emptyStart, int emptyLength)[] expectedEmptyParts)
//        {
//            var partition = new Partition<int>(length);

//            for (var newId = 0; newId < parts.Length; newId++)
//            {
//                var (partStart, partLength) = parts[newId];
//                partition.AddNewPart(newId, partStart, partLength);
//            }

//            var emptyParts = partition.EmptyParts.ToArray();

//            Assert.AreEqual(expectedEmptyParts.Length, emptyParts.Length,
//                "Expected and actual empty part count are not equal");

//            for (var emptyPartInd = 0; emptyPartInd < emptyParts.Length; emptyPartInd++)
//            {
//                var emptyPart = emptyParts[emptyPartInd];
//                var expectedPart = expectedEmptyParts[emptyPartInd];

//                Assert.AreEqual(
//                    (expectedPart.emptyStart, expectedPart.emptyLength),
//                    (emptyPart.Start, emptyPart.Length)
//                );
//            }
//        }
//    }
//}