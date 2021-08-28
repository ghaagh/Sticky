using System.Threading.Tasks;

namespace Sticky.Domain.SegmentAggrigate
{
    public interface ISegmentRepository
    {
        Task<Segment> FindByIdAsync(long id);
    }
}
