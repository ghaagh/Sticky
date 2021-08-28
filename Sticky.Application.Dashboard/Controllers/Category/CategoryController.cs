using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Application.Dashboard.Controllers.Category.Dto;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{HostId}/{Keyword}")]
        public async Task<IActionResult> Search([FromRoute] CategorySearchRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
