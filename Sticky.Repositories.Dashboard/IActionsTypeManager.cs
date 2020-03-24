using Sticky.Dto.Dashboard.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard
{
    public interface IActionTypeManager
    {
        Task<List<ActionResult>> GetActionListAsync();
    }
}
