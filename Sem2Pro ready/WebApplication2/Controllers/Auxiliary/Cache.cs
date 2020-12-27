using System;
using System.Collections.Generic;

namespace UserLayer.Controllers
{
    public class Cache
    {
        public Dictionary<Guid, byte[]> Storadge { get; } 
            = new Dictionary<Guid, byte[]>();

        public byte[] Get(Guid id)
        {
            try
            {
                return Storadge[id];
            }
            catch (KeyNotFoundException exception)
            {
                throw new CacheException("No item in cache", exception);
            }
        }

        public void Add(Guid id, byte[] value)
        {
            try
            {
                Storadge.Add(id, value);
            }
            catch (ArgumentException exception)
            {
                throw new CacheException("Item already in cache", exception);
            }
        }
    }
}