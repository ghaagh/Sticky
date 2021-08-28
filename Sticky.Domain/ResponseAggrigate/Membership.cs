using System.Collections.Generic;

namespace Sticky.Domain.ResponseAggrigate
{
    public class Membership
    {
        public Membership()
        {
        }
        public long SegmentId { get; set; }
        public long HostId { get; set; }
        public int Priority { get; set; }
        public List<MemberShipProduct> Products { get; set; } = new List<MemberShipProduct>();
    }
}
