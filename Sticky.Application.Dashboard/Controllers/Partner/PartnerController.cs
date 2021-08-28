using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Application.Dashboard.Controllers.Partner.Dto;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Partner
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartnerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PartnerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePartnerRequest hostRequest)
        {
            var result = await _mediator.Send(hostRequest);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> AccessToUser(long id,ModifyPartnerRequest accessHostRequest)
        {
            accessHostRequest.Id = id;
            var result = await _mediator.Send(accessHostRequest);
            return Ok(result);
        }
    }
}
