using System;

namespace Sticky.Infrastructure.Redis
{
    public class RedisConfig
    {
        public RedisItemConfig ProductCache { get; set; }
        public RedisItemConfig HostCache { get; set; }
        public RedisItemConfig PartnerCache { get; set; }
        public RedisItemConfig SegmentCache { get; set; }
    }

    public enum CacheType
    {
        Key,
        Hashset
    }

    public class RedisItemConfig
    {
        public CacheType CacheType { get; set; }
        public int DatabaseNumber { get; set; }
        public TimeSpan? Expire { get; set; }
    }
}
