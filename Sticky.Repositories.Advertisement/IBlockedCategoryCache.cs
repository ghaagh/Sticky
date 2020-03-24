using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// a redis wrapper for blocked category cache.
    /// </summary>
    public interface IBlockedCategoryCache
    {
        /// <summary>
        /// returns list of blocked cateogries in string format .
        /// </summary>
        /// <param name="id">id of host</param>
        /// <returns>
        /// list of blocked category
        /// </returns>
        Task<List<string>> GetBlockedCategoriesForHostAsync(int hostId);
    }
}
