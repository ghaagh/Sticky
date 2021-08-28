using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Application.Dashboard.Controllers.Segment.Dto;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Segment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SegmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SegmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSegment(CreateSegmentRequest hostRequest)
        {
            var result = await _mediator.Send(hostRequest);
            return Ok(result);
        }
    }
}
