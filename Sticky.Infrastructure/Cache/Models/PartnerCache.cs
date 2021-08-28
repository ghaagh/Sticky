namespace Sticky.Infrastructure.Cache.Models
{
    public class PartnerCache : CacheModel
    {
        public long Id { get; set; }
        public string PartnerName { get; set; }
        public string ParnerHash { get; set; }
        public string Domain { get; set; }
        public string CookieSyncAddress { get; set; }
        public bool? Verified { get; set; }
    }
}
