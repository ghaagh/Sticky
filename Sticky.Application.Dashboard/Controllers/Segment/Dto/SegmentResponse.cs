using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Application.Dashboard.Controllers.Segment.Dto
{
    public class SegmentResponse
    {
        public string SegmentName { get; set; }
        public ActivityTypeEnum ActivityType { get; set; }
        public ActionTypeEnum ActionType { get; set; }
        public long HostId { get; set; }
        public bool Public { get; set; }
        public bool Paused { get; set; }
        public string ActivityExtra { get; set; }
        public string ActionExtra { get; set; }
        public long TotalMembership { get; set; }
        public long TotalClicks { get; set; }

    }
}
