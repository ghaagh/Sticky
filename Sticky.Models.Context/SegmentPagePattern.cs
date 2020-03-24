using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class SegmentPagePattern
    {
        public long Id { get; set; }
        public string PatternName { get; set; }
        public string PagePattern { get; set; }
        public int HostId { get; set; }
        public string CreatorUserId { get; set; }
        public bool Deleted { get; set; }

        public virtual User CreatorUser { get; set; }
        public virtual Host Host { get; set; }
    }
}
