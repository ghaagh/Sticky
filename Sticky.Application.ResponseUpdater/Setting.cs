using Sticky.Domain.ResponseAggrigate;

namespace Sticky.Application.ResponseUpdater
{
    public class Setting
    {
        public string Redis { get; set; }
        public string DruidClient { get; set; }
        public int? HostId { get; set; }
        public long? SegmentId { get; set; }
        public int BatchSize { get; set; }
        public string StatType { get; set; }
        public string AerospikeClient { get; set; }
        public int EmptyExpirationInMinutes { get; set; }
        public int FullExpirationInMinutes { get; set; }
        public ResponseUpdaterTypeEnum ResponseUpdaterType { get; set; }
    }
}
