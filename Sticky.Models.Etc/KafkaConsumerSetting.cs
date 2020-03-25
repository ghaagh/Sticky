using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Etc
{

    public class KafkaConsumerSetting
    {
        public string AerospikeAddress { get; set; }
        public int AerospikePort { get; set; }
        public string KafkaAddress { get; set; }
        public int KafkaPort { get; set; }
        public int MaxProductRecordPerUser { get; set; }
    }
}
