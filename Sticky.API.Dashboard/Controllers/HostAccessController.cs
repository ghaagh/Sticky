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
    [Route("api/HostAccess")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class HostAccessController : ControllerBase
    {
        private readonly IHostManager _hostManager;
        private readonly UserManager<User> _userManager;
        public HostAccessController(IHostManager hostManager,UserManager<User> userManager)
        {
            _hostManager = hostManager;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<EmptyResponse> AddAccess([FromBody]AdHostAccessRequest request)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _userManager.FindByEmailAsync(request.TargetEmail??"");
            if(user==null)
                 return new EmptyResponse() { Valid = false, Error = "no access" };
            var isOk =  await _hostManager.GrantAccessToHostAsync(userId, request.TargetEmail, (int)request.HostId);
            if (isOk)
                return new EmptyResponse() { Valid = true };
            return new EmptyResponse() { Valid = false, Error = "no access" };
        }
    }
}