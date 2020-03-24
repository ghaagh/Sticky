using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace Sticky.Models.Redis
{
    [ZeroFormattable]
    public class UserSegment
    {
        [Index(0)]
        public virtual int HostId { get; set; }
        [Index(1)]
        public virtual int SegmentId { get; set; }
        [Index(2)]
        public virtual int? ResultTypeId { get; set; }
        [Index(3)]
        public virtual string ExtraQuery { get; set; }
        [Index(4)]
        public virtual List<string> Sizes { get; set; } = new List<string>();
        [Index(5)]
        public virtual int SegmentPriority { get; set; }
        [Index(6)]
        public List<HostProduct> Products { get; set; } = new List<HostProduct>();
    }
}
