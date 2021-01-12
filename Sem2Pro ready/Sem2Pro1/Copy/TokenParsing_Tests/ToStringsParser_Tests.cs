//using NUnit.Framework;
//using TokenParsing;

//namespace TokenParsing_Tests
//{
//    public class ToStringsParserTests
//    {
//        [TestCase("x^2 + 1", "x", "^", "2", "+", "1")]
//        [TestCase("sin3x+1", "sin", "3", "x", "+", "1")]
//        //[TestCase("x1 + x2 where x1 = 4, x2 = 5")]
//        [TestCase("3sin5+7", "3", "sin", "5", "+", "7")]
//        [TestCase("x^2 + t where t = 1", "x", "^", "2", "+", "t")]
//        public void TestSimplePoly(string input, params string[] expectedResult)
//        {
//            TestParsing(input, expectedResult);
//        }

//        private void TestParsing(string input, params string[] expectedResult)
//        {
//            var parser = new ToStringsParser(input);
//            var actualMainFunction = parser.GetResult().MainFunction;
//            CollectionAssert.AreEqual(expectedResult, actualMainFunction);
//        }
//    }
//}