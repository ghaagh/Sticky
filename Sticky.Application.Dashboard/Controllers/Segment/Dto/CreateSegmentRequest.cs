
using MediatR;
using Sticky.Domain.SegmentAggrigate;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Segment.Dto
{
    public class CreateSegmentRequest : IRequest<SegmentResponse>
    {
        [Required]
        public string SegmentName { get; set; }
        [Required]
        public ActivityTypeEnum ActivityType { get; set; }
        [Required]
        public ActionTypeEnum ActionType { get; set; }
        [Required]
        public int HostId { get; set; }

        public string ActivityExtra { get; set; }
        public string ActionExtra { get; set; }
    }
}
