using Microsoft.AspNetCore.Mvc;

using Sticky.Infrastructure.Message;
using Sticky.Application.Script.Controllers.Dto;
using Sticky.Application.Script.Services;
using System.Threading.Tasks;

namespace Sticky.Application.Script.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionController : ControllerBase
    {
        private readonly IUtility _domainExtractor;
        private readonly IMessage _messager;

        public ActionController(IUtility domainExtractor,IMessage messager)
        {
            _domainExtractor = domainExtractor;
            _messager = messager;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActionRequest request)
        {
            var origin = Request.Headers["Origin"].ToString();
            var hostAddress = _domainExtractor.ExtractDomain(origin);
            foreach(var productId in request.ProductId)
            {
                await _messager.SendActionMessageAsync(hostAddress, request.UserId, productId, request.StatType);
            }
            return Ok();
        }
    }
}
