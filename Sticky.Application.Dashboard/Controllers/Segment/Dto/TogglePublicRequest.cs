using MediatR;


namespace Sticky.Application.Dashboard.Controllers.Segment.Dto
{
    public class TogglePublicRequest : IRequest<SegmentResponse>
    {
        public long SegmentId { get; set; }
    }
}
