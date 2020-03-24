using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ActionTypes")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class ActionTypesController : ControllerBase
    {
        private readonly IActionTypeManager _actionTypeManager;
        public ActionTypesController(IActionTypeManager actionTypeManager)
        {
            _actionTypeManager = actionTypeManager;
        }

        [HttpGet]
        public async Task<ActionResponse> List()
        {
            var response = new ActionResponse() { Valid = true };
            response.Result = await _actionTypeManager.GetActionListAsync();
            return response;
        }
    }
}