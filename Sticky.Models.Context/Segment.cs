using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Segment
    {
        public Segment()
        {
            ProductTextTemplates = new HashSet<ProductTextTemplate>();
            SegmentStaticNatives = new HashSet<SegmentStaticNative>();
            Clicks = new HashSet<Click>();
        }

        public int Id { get; set; }
        public int HostId { get; set; }
        public string SegmentName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatorId { get; set; }
        public int AudienceId { get; set; }
        public int ActionId { get; set; }
        public string AudienceExtra { get; set; }
        public string ActionExtra { get; set; }
        public bool Paused { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublic { get; set; }
        public long? AudienceNumber { get; set; }
        public virtual ActionType Action { get; set; }
        public virtual AudienceType Audience { get; set; }
        public virtual Host Host { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<ProductTextTemplate> ProductTextTemplates { get; set; }
        public virtual ICollection<SegmentStaticNative> SegmentStaticNatives { get; set; }
        public virtual ICollection<Click> Clicks { get; set; }
    }
}
