using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Domain.StatAggrigate
{
    public partial class ClickStatistic : StatisticEntity
    {
        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; } 
    }
}
