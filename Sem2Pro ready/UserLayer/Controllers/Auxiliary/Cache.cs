using System;
using System.Collections.Concurrent;

namespace UserLayer.Controllers.Auxiliary
{
    public class Cache
    {
        private ConcurrentDictionary<Guid, byte[]> Storage { get; } 
            = new ConcurrentDictionary<Guid, byte[]>();

        public bool TryGet(Guid id, out byte[] value)
        {
            if (!Storage.TryGetValue(id, out value))
            {
                return false;
            }
            return true;
        }

        public bool TryAdd(Guid id, byte[] value)
        {
            if (!Storage.TryAdd(id, value))
                return false;
            return true;
        }
    }
}