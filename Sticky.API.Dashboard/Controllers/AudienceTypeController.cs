using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/AudienceTypes")]
    [Authorize(Roles=Roles.HostOwnerOrAdmin)]
    public class AudienceTypesController : ControllerBase
    {
        private readonly IAudienceTypeManager _audienceTypeManager;
        public AudienceTypesController(IAudienceTypeManager audienceTypeManager)
        {
            _audienceTypeManager = audienceTypeManager;
        }
        [HttpGet]
        public async Task<AudienceTypeResponse> List()
        {
            var response = new AudienceTypeResponse() { Valid = true };
            response.Result = await _audienceTypeManager.GetAudienceListAsync();
            return response;
        }
    }
}