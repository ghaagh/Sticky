using System.Collections.Generic;

namespace Sticky.Dto.Advertisement.Response
{
    public class MemberShipResult
    {
        public MemberShipResult()
        {
        }
        public long SegmentId { get; set; }
        public int HostId { get; set; }
        public int Priority { get; set; }
        public List<NativeDetails> Products { get; set; } = new List<NativeDetails>();
    }
}
