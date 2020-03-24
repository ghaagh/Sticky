using System.Collections.Generic;
using System.Threading.Tasks;
using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;

namespace Sticky.Repositories.Dashboard
{
    public interface ISegmentManager
    {
        Task<bool> UpdateNativesAsync(string userId,SegmentNativeRequest request);
        Task<Segment> CreateSegmentAsync(string userId,CreateSegmentRequest request);
        Task<bool> PlayPauseSegmentAsync(int segmentId);
        Task<bool> DeleteSegmentAsync(int segmentId);
        Task<List<SegmentResult>> GetUserSegmentsAsync(string userId,int hostId);
        Task<List<SegmentResult>> GetPublicSegmentsAsync(string userId);
        Task<SegmentResult> GetByIdAsync(int segmentId);
        Task<bool> PublicAccessToggleAsync(string userId, int segmentId);

    }
}
