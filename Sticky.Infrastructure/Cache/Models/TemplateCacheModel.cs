namespace Sticky.Infrastructure.Cache.Models
{
    public class TemplateCache: CacheModel
    {
        public long Id { get; set; }
        public string Template { get; set; }
        public long SegmentId { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

    }
}
