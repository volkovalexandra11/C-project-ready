using System;
using System.Runtime.Serialization;

namespace Infrastructure.TopDowns
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }
}