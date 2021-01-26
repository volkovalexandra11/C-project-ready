using System;

namespace UserLayer.Controllers
{
    public class CacheException : Exception
    {
        public CacheException(string message) 
            : base(message)
        { }
    }
}