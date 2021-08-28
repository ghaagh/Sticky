using Sticky.Domain.SegmentAggrigate;
using System.Collections.Generic;

namespace Sticky.Infrastructure.Cache.Models
{
    public class SegmentCache: CacheModel
    {
        public long Id { get; set; }
        public string SegmentName { get; set; }
        public ActivityTypeEnum ActivityType { get; set; }
        public ActionTypeEnum ActionType { get; set; }
        public long HostId { get; set; }
        public bool Public { get; set; }
        public bool Paused { get; set; }
        public string ActivityExtra { get; set; }
        public string ActionExtra { get; set; }

        public IEnumerable<TemplateCache> Templates { get; set; }
    }
}
