using System;

namespace UserLayer.Controllers
{
    public class DrawingException : Exception
    {
        public DrawingException(string message) : base(message)
        {
        }

        public DrawingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}