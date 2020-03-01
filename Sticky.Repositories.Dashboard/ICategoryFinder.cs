using Sticky.Dto.Dashboard.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard
{
    public interface ICategoryFinder
    {
        Task<List<CategorytResult>> FindMatchedCategoriesAsync(int hostId, string keyword);
    }
}
