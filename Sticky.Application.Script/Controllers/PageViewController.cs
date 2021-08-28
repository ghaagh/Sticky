using Microsoft.AspNetCore.Mvc;
using Sticky.Infrastructure.Message;
using Sticky.Application.Script.Controllers.Dto;
using Sticky.Application.Script.Services;
using System.Threading.Tasks;

namespace Sticky.Application.Script.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageViewController : ControllerBase
    {
        private readonly IUtility _utility;
        private readonly IMessage _messager;

        public PageViewController(IUtility utility, IMessage messager)
        {
            _utility = utility;
            _messager = messager;
        }
        [HttpPost]
        public async Task<ActionResult> Post(PageViewRequest request)
        {
            var hostAddress = _utility.ExtractDomain(Request.Headers["Origin"].ToString());
            await _messager.SendPageViewMessageAsync(hostAddress, request.UserId, request.Address);
            return Ok();
        }
    }
}
