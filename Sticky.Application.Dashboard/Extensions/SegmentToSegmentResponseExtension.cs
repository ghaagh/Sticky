using Sticky.Application.Dashboard.Controllers.Segment.Dto;
using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Application.Dashboard.Extensions
{
    public static class SegmentToSegmentResponseExtension
    {
        public static SegmentResponse ToSegmentResponse(this Segment segment)
        {
            return new SegmentResponse()
            {
                SegmentName = segment.Name,
                ActionExtra = segment.ActionExtra,
                ActionType = segment.Action,
                ActivityType = segment.Activity,
                ActivityExtra = segment.ActivityExtra,
                HostId = segment.HostId,
                Public = segment.IsPublic,
                Paused = segment.Paused,

            };
        }
    }
}
