using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Message.Kafka
{
    public class KafkaConfig
    {
        public string Address { get; set; }
        public string TopicName { get; set; }
    }
}
