using Microsoft.AspNetCore.Mvc;
using Sticky.Application.Dashboard.Controllers.Account.Dto;
using MediatR;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var registerResult = await _mediator.Send(registerRequest);
            return Ok(registerResult);
        }

        [HttpPost("token")]
        public async Task<ActionResult> Login(GetTokenRequest getTokenRequest)
        {
            var registerResult = await _mediator.Send(getTokenRequest);
            return Ok(registerResult);
        }
    }
}
