using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using TokenParsing.Tokens.LowLevelEntities.ClassicOperationTokens;

namespace TokenParsing
{
    //    public enum TokenType
    //    {
    //        Constant,
    //        UnaryOp,
    //        BinaryOp,
    //    }

    //    public enum BinaryNotationType
    //    {
    //        Infix,
    //        Suffix,
    //        Postfix
    //    }

    //    public interface IToken
    //    {
    //        TokenType Type { get; }
    //        ExpressionType Operation { get; }
    //        Expression ToExpression(params Expression[] parameters);
    //    }

    //    static class Tokens
    //    {
    //        private static List<IToken> KnownTokens = new List<IToken>
    //        {
    //            new BinaryToken("*", ExpressionType.Multiply),
    //            new BinaryToken("/", ExpressionType.Divide),
    //            new BinaryToken("+", ExpressionType.Add),
    //            new BinaryToken("-", ExpressionType.Subtract),
    //            new UnaryToken("-", ExpressionType.Negate),
    //            //new OperationToken("tg", ExpressionType.Call)
    //        };

    //        public IToken GetToken(string token)
    //        {
    //            if (double.TryParse(token, out var res))
    //                return new ConstantToken(res);
    //            if 
    //        }
    //    }

    //    class ExpressionParser
    //    {
    //        private string[] Tokens { get; }
    //        private Stack<Expression> ParsedExpressions { get; } 

    //        private int TokensInd { get; set; }

    //        public ExpressionParser(string[] tokens)
    //        {
    //            ParsedExpressions = new Stack<Expression>();
    //            Tokens = tokens;
    //        }
    //        public Expression Parse(string[] tokens)
    //        {
    //            while (true)
    //            {
    //                ParseAt(TokensInd);
    //            }
    //        }

    //        public void ParseAt(int ind)
    //        {
    //            var nextToken = Tokens[ind];
    //            if nextToken
    //        }

    //        private Expression GetTokenFrom(int ind)
    //        {

    //        }
    //    }
    class Program
    {
        //static void Main(string[] args)
        //{
        //    var expr = "1+7*8/6-5/7-4*2*3/4+1".Select(c => c.ToString()).ToArray();
        //    var parsed = new SimpleParser(expr).Parse();
        //    Console.WriteLine(parsed);
        //}

        static void Main()
        {
            //var expr = new IToken[]
            //{
            //    new ConstantToken(1), new AdditionToken(),
            //    new ConstantToken(2), new MultiplicationToken(), new ConstantToken(3)
            //};
            //var parsed = new TokenToExprParser(expr, null).Parse();
            //Console.WriteLine(parsed);
        }
    }
}
