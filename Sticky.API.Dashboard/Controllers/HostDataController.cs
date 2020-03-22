using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;

namespace DashboardAPI.Controllers
{
    [Route("api/HostData")]
    [Produces("application/json")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class HostDataController : ControllerBase
    {
        private readonly IHostDataExtractor _hostDataExtractor;
        public HostDataController(IHostDataExtractor hostDataExtractor)
        {
            _hostDataExtractor = hostDataExtractor;
        }
        [HttpGet("{id}")]
        public async Task<HostDataResponse> Get(int id)
        {
            try
            {
                var model = new HostDataResponse()
                {
                    Result = await _hostDataExtractor.ExtractHostInfoAsync(id),
                    Valid = true
                };
                return model;
            }
            catch (Exception ex)
            {
                return new HostDataResponse() { Error = ex.Message + (ex.InnerException == null ? "" : ex.InnerException.Message) };
            }


        }
    }
}