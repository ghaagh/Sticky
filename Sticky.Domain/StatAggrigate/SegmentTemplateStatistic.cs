namespace Sticky.Domain.StatAggrigate
{
    public partial class SegmentTemplateStatistic : StatisticEntity
    {
        public int SegmentId { get; set; }
        public int? TemplateId { get; set; }
        public int Clicks { get; set; }
    }
}
