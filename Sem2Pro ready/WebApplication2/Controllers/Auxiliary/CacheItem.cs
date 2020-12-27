using System;

namespace UserLayer.Controllers
{
    public class CacheItem
    {
        public CacheItem(int id, byte[] content)
        {
            Id = id;
            Content = content;
        }

        public int Id { get; }
        public byte[] Content { get; }
    }
}