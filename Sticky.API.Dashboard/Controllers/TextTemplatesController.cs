using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/TextTemplates")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class TextTemplatesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITextTemplateManager _textTemplateManager;
        public TextTemplatesController( ITextTemplateManager textTemplateManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _textTemplateManager = textTemplateManager;
        }

        [HttpGet("{segmentId}")]
        public async Task<TextTemplateResponseV2> List(int segmentId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var response = new TextTemplateResponseV2() { Valid = true };
            response.Result = await _textTemplateManager.GetTemplateAsync(userId, segmentId);
            return response;
        }

        [HttpPost]
        public async Task<EmptyResponse> Update([FromBody]UpdateTextTemplateRequest request)
        {
            var result = await _textTemplateManager.UpdateTemplateAsync(request);
            return new EmptyResponse() { Valid = result };
        }
    }
}