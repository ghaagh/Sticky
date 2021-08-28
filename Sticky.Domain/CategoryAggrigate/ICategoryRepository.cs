using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Domain.CategoryAggrigate
{
    public interface ICategoryRepository
    {
        Task<List<Category>> SearchAsync(long hostId, string keywrord);
    }
}
