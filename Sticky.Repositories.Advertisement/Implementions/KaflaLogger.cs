using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Options;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class KafkaLogger : IKafkaLogger
    {
        private readonly IRedisCache _redisCache;
        private readonly Producer<Null, string> _producer;
        public KafkaLogger(IOptions<ScriptAPISetting> configuratoin, IRedisCache redisCache)
        {
            _redisCache = redisCache;
            var kafkaaddresses = configuratoin.Value.KafkaAddress;
            var config = new Dictionary<string, object>
      {
        { "bootstrap.servers", kafkaaddresses}
      };

            _producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }
        public KafkaLogger(IRedisCache redisCache)
        {
            _redisCache = redisCache;
            var config = new Dictionary<string, object>
      {
        { "bootstrap.servers", "<##Change:KafkaAddress##>"}
      };
            _producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }
        public async Task SendMessage(DruidData message)
        {

         await _producer.ProduceAsync("Sticky", null, Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }


    }
}
