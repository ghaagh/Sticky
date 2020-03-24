using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Redis
{
    public class MembershipData
    {
        public long StickyUserId { get; set; }
        public List<UserSegment> Segments { get; set; } = new List<UserSegment>();
    }
}
