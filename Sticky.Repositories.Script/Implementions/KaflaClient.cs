using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Script.Implementions
{
    public class KafkaClient : IKafkaClient
    {
        private readonly IRedisCache _redisCache;
        private readonly IProducer<Null, string> _producer;
        public KafkaClient(IOptions<ScriptAPISetting> configuratoin, IRedisCache redisCache)
        {
            _redisCache = redisCache;
            var kafkaaddresses = configuratoin.Value.KafkaAddress;
            var config = new ProducerConfig { BootstrapServers = kafkaaddresses };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public KafkaClient(IRedisCache redisCache)
        {
            _redisCache = redisCache;
            var config = new ProducerConfig { BootstrapServers = "<##Change:KafkaAddress##>" };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task SendMessage(DruidData message)
        {

         await _producer.ProduceAsync("Sticky",new Message<Null, string> { Value = Newtonsoft.Json.JsonConvert.SerializeObject(message) });
        }


    }
}
