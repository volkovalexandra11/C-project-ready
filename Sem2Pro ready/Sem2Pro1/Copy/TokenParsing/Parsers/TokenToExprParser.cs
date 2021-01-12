using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TokenParsing
{
    public class TokenToExprParser
    {
        public IToken[] Tokens { get; }
        public bool LastTokenIsntOperation { get; set; }

        public HashSet<ParameterExpression> Parameters { get; }
        //public ... ParameterExpressions { get; }

        public Stack<Expression> ExpressionStack { get; }

        public TokenToExprParser(IToken[] tokens, HashSet<ParameterExpression> parameters)
        {
            Tokens = tokens;
            Parameters = parameters;
            ExpressionStack = new Stack<Expression>();
        }

        public Expression Parse()
        {
            ParseFrom(0, (int)BasePriority.Min, out var endInd);
            if (endInd != Tokens.Length)
                throw new InvalidOperationException("Didnt reach end of expression");
            if (ExpressionStack.Count > 1)
                throw new InvalidOperationException("Unexpected tokens");
            if (ExpressionStack.Count == 0)
                throw new InvalidOperationException("Not enough tokens");
            return ExpressionStack.Pop();
        }

        private void ParseFrom(int startInd, Predicate<IToken> isTerminatingToken, out int endInd)
        {
            if (startInd >= Tokens.Length)
                throw new IndexOutOfRangeException("Unexpected end of expression");

            endInd = startInd;

            while (endInd < Tokens.Length && !isTerminatingToken(Tokens[endInd]))
            {
                Tokens[endInd].Parse(this, endInd, out endInd);
            }
        }

        internal void ParseFrom(int startInd, int minPriority, out int endInd)
        {
            if (startInd >= Tokens.Length)
                throw new IndexOutOfRangeException("Unexpected end of expression");

            endInd = startInd;

            while (
                endInd < Tokens.Length &&
                !(Tokens[endInd] is BinaryInfixToken binaryToken && binaryToken.Priority < minPriority)
            )
            {
                Tokens[endInd].Parse(this, endInd, out endInd);
            }
        }

        internal void ParseBracket(int startInd, BracketType brType, out int endInd)
        {
            if (startInd >= Tokens.Length)
                throw new IndexOutOfRangeException("Unexpected end of expression");

            endInd = startInd;

            while (
                endInd < Tokens.Length &&
                !(Tokens[endInd] is BracketToken bracketToken
                  && bracketToken.Type == brType
                  && bracketToken.Position == BracketPosition.Closing)
            )
            {
                Tokens[endInd].Parse(this, endInd, out endInd);
            }
        }
    }
}