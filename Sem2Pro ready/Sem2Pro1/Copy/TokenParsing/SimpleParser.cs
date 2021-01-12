//using System.Collections.Generic;
//using System.IO;
//using System.Linq.Expressions;
//using System.Security.Cryptography.X509Certificates;

//namespace TokenParsing
//{
//    public class SimpleParser
//    {
//        private static Dictionary<char, int> Priorities = new Dictionary<char, int>
//        {
//            ['+'] = 0,
//            ['-'] = 0,
//            ['*'] = 1,
//            ['/'] = 1,
//            ['#'] = 2,
//            ['^'] = 3
//        };


//        private string[] Tokens { get; }
//        private int CurrPos;

//        public SimpleParser(string[] tokens) => Tokens = tokens;

//        public Expression Parse()
//        {
//            var constStack = new Stack<Expression>();
//            var opStack = new Stack<ExpressionType>();
//            var priorityStack = new Stack<int>();
            
//            while (CurrPos < Tokens.Length)
//            {
//                if (double.TryParse(Tokens[CurrPos], out var newConst))
//                {
//                    constStack.Push(Expression.Constant(newConst));
//                    CurrPos++;
//                    continue;
//                }
//                var cmd = Tokens[CurrPos][0];
//                var cmdPr = Priorities[cmd];
//                CollapseTop(constStack, opStack, priorityStack, Priorities[cmd]);
//                priorityStack.Push(cmdPr);
//                if (cmd == '+')
//                {
//                    opStack.Push(ExpressionType.Add);
//                }
//                else if (cmd == '-')
//                {
//                    opStack.Push(ExpressionType.Subtract);
//                }
//                else if (cmd == '*')
//                {
//                    opStack.Push(ExpressionType.Multiply);
//                }
//                else if (cmd == '/')
//                {
//                    opStack.Push(ExpressionType.Divide);
//                }
//                CurrPos++;
//            }
//            CollapseTop(constStack, opStack, priorityStack, -1);
//            return constStack.Pop();
//        }

//        //private Expression ParseSubExpression(int start, out int end)
//        //{

//        //}

//        private static void CollapseTop(
//            Stack<Expression> constStack, Stack<ExpressionType> opStack, Stack<int> prStack,
//            int newPriority
//        )
//        {
//            while (prStack.TryPeek(out var topPriority) && topPriority >= newPriority)
//            {
//                var (top, subTop) = (constStack.Pop(), constStack.Pop());
//                prStack.Pop();
//                var newExpr = Expression.MakeBinary(opStack.Pop(), subTop, top);
//                constStack.Push(newExpr);
//            }
//        }
//    }
//}