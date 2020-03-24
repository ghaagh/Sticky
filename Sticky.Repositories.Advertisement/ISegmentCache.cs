using Sticky.Models.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// A redis wrapper for getting segment infos such as filters, tags and so on.
    /// </summary>
    public interface ISegmentCache
    {

       // Task<SegmentStaticNative> FindSegmentNativeAsync(int segmentId);
        /// <summary>
        /// returns basic segment info based on the given Id.
        /// </summary>
        /// <param name="segmentId">
        /// id of the segment.
        /// </param>
        /// <returns></returns>
        Task<RedisSegment> FindSegmentAsync(int segmentId);
        /// <summary>
        /// returns a list of all active segments.
        /// </summary>
        /// <returns></returns>
        Task<List<RedisSegment>> GetAllActiveSegments();
    }
}
