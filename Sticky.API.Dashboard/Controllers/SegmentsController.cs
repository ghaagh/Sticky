using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;
using Sticky.Dto.Dashboard.Response;
using Sticky.Dto.Dashboard.Request;
using Microsoft.AspNetCore.Identity;
using Sticky.Models.Context;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Segments")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class SegmentsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISegmentManager _segmentManager;
        public SegmentsController(ISegmentManager segmentManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _segmentManager = segmentManager;
        }
        [HttpGet]
        public async Task<SegmentResponse> List(string email, int? id)
        {
            var response = new SegmentResponse();
            var patterns = await _segmentManager.GetUserSegmentsAsync(email, id ?? 0);
            response.Valid = true;
            response.Result = patterns;
            return response;
        }
        [HttpGet("Public")]
        public async Task<SegmentResponse> ListAllPublic(string email)
        {
            var response = new SegmentResponse();
            var patterns = await _segmentManager.GetPublicSegmentsAsync(email);
            response.Valid = true;
            response.Result = patterns;
            return response;
        }
        [HttpGet("{id}")]
        public async Task<SegmentResponse> Get(int id)
        {
            var response = new SegmentResponse();
            var pattern = await _segmentManager.GetByIdAsync(id);
            response.Valid = true;
            response.Result.Add(pattern);
            return response;
        }
        [HttpPost]
        public async Task<EmptyResponse> Create([FromBody]CreateSegmentRequest model)
        {
            var response = new EmptyResponse();
            var userId = _userManager.GetUserId(HttpContext.User);
           
            await _segmentManager.CreateSegmentAsync(userId,model); ;
            response.Valid = true;
            return response;
        }
        [HttpDelete("{id}")]
        public async Task<EmptyResponse> Delete(int id)
        {
            var response = new PagePatternsReponse();
            await _segmentManager.DeleteSegmentAsync(id);
            response.Valid = true;
            return response;
        }
        [HttpPatch("{id}")]
        public async Task<EmptyResponse> PlayPause(int id)
        {
            var response = new PagePatternsReponse();
            await _segmentManager.PlayPauseSegmentAsync(id);
            response.Valid = true;
            return response;
        }

    }
}