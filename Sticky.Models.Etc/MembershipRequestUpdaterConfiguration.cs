using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Models.Etc
{
    /// <summary>
    /// Contains All Changable Configuration from app.setting.
    /// </summary>

    public class MembershipRequestUpdaterConfiguration
    {
        public string MongoConnectionString { get; set; }
        public int EmptyExpirationInMinutes { get; set; }
        public string DruidClient { get; set; }
        public int? HostId { get; set; }
        public int? SegmentId { get; set; }
        public int BatchSize { get; set; }
        public bool Query { get; set; }
        public string StatType { get; set; }
        public bool Categories { get; set; }
        public string AerospikeClient { get; set; }
        public int FullExpirationInMinutes { get; set; }
    }
    public class DruidSegmentationConfiguration
    {
        public string DruidAPI { get; set; }
    }


}
