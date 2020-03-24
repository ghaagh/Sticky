using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sticky.Repositories.Dashboard;
using Sticky.Dto.Dashboard.Response;
using Microsoft.AspNetCore.Authorization;
using Sticky.Models.Etc;
using Microsoft.AspNetCore.Identity;
using Sticky.Models.Context;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/SegmentPublicAccessToggle")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class SegmentPublicAccessToggleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISegmentManager _segmentManager;
        public SegmentPublicAccessToggleController(ISegmentManager segmentManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _segmentManager = segmentManager;
        }
        [HttpPost("{id}")]
        public async Task<EmptyResponse> Toggle(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return new EmptyResponse() { Valid = false, Error = "No Access" };
            var response = new EmptyResponse();
            var isok=  await _segmentManager.PublicAccessToggleAsync(user.Id,id) ;
            response.Valid = isok;
            return response;
        }
    }
}