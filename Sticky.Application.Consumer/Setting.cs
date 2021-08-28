namespace Sticky.Application.Consumer
{
    class Setting
    {
        public string Redis { get; set; }
        public string AerospikeAddress { get; set; }
        public int AerospikePort { get; set; }
        public string KafkaAddress { get; set; }
        public int KafkaPort { get; set; }
        public int MaxProductRecordPerUser { get; set; }
    }
}
