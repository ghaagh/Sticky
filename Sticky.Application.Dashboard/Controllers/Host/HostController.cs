using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sticky.Application.Dashboard.Controllers.Host.Dto;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Host
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateHost(CreateHostRequest hostRequest)
        {
            var result = await _mediator.Send(hostRequest);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> AccessToUser(AccessHostRequest accessHostRequest)
        {
            var result = await _mediator.Send(accessHostRequest);
            return Ok(result);
        }
    }
}
