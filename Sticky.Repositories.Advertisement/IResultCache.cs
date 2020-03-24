using Sticky.Models.Mongo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// Redis Cache for user specific or general product results for an active segment.
    /// </summary>
    public interface IResultCache
    {
        /// <summary>
        /// returns a list of products base on the given key.
        /// </summary>
        /// <param name="key">
        /// 1 or 2 parts. contains UserId and segmentId(like : userId%SegmentId) or just segmentId(for general segments.)
        /// </param>
        /// <returns></returns>
        Task<List<HostProduct>> FindResultAsync(string key);
    }
}
