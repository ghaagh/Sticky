using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/HostCategories")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class HostCategoriesController : ControllerBase
    {
        private readonly ICategoryFinder _categoryFinder;
        public HostCategoriesController(ICategoryFinder categoryFinder)
        {
            _categoryFinder = categoryFinder;
        }
        [HttpGet]
        public async Task<CategoryReponse> List(int hostId,string keyword)
        {

            var filterResponse = new CategoryReponse
            {
                Valid = true,
                Result = (await _categoryFinder.FindMatchedCategoriesAsync(hostId, keyword)).ToList()
            };
            return filterResponse;
        }
    }
}