using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Sticky.Domain.HostAggrigate.Exceptions;
using Sticky.Domain.SegmentAggrigate;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Sticky.Infrastructure.Message.Kafka
{
    class KafkaMessager : IMessage
    {
        private readonly IProducer<Null, string> _producer;
        private readonly KafkaConfig _kafkaConfig;
        private readonly IMultipleCache<HostCache> _hostCache;
        private readonly ICache<ProductCache> _productCache;
        public KafkaMessager(IOptions<KafkaConfig> kafkaConfig,IMultipleCache<HostCache> hostCache,ICache<ProductCache> productCache)
        {
            _kafkaConfig = kafkaConfig.Value;
            _productCache = productCache;
            _hostCache = hostCache;

            var config = new Dictionary<string, string> { { "bootstrap.servers", _kafkaConfig.Address },{ "delivery.timeout.ms","2000" } };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendActionMessageAsync(string host, long user, string productId, ActivityTypeEnum statType)
        {
            var cachedHost = await _hostCache.GetAsync(host);
            if (cachedHost == null)
                return;
            var cachedProduct = await _productCache.GetAsync($"{cachedHost.Host}_{productId}");
            if (cachedProduct == null)
                return;
            var message = new Message<Null, string>()
            {
                Key = null,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new MessageModel()
                {
                    CategoryName = cachedProduct.CategoryName,
                    Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    HostName = cachedHost.Host,
                    ImageAddress = cachedProduct.ImageAddress,
                    Price = cachedProduct.Price,
                    ProductName = cachedProduct.ProductName,
                    PageAddress = cachedProduct.Url,
                    ProductId = productId,
                    StatType = statType.ToString(),
                    UserId = user.ToString(),
                    HostId = cachedHost.Id.ToString(),

                })
            };
            await _producer.ProduceAsync(_kafkaConfig.TopicName, message);
        }

        public async Task SendPageViewMessageAsync(string host, long user, string address)
        {
            var cachedHost = await _hostCache.GetAsync(host);
            if (cachedHost == null)
                return;
            var message = new Message<Null, string>()
            {
                Key = null,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new MessageModel()
                {
                    CategoryName = string.Empty,
                    PageAddress = address,
                    Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    HostName = cachedHost.Host,
                    UserId = user.ToString(),
                    HostId = cachedHost.Id.ToString(),

                })
            };
            await _producer.ProduceAsync(_kafkaConfig.TopicName, message);
        }

        public async Task SendProductMessageAsync(string host, long user, string productId, string productname, string imageAddress, string category, string url, int price)
        {
            var cachedHost = await _hostCache.GetAsync(host);
            if (cachedHost == null)
                throw new HostNotFoundException();
            var message = new Message<Null, string>()
            {
                Key = null,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new MessageModel()
                {
                    CategoryName = category,
                    Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    HostName = cachedHost.Host,
                    ImageAddress = imageAddress,
                    Price = price,
                    ProductName = productname,
                    PageAddress =url,
                    ProductId = productId,
                    StatType = ActivityTypeEnum.VisitProduct.ToString(),
                    UserId = user.ToString(),
                    HostId = cachedHost.Id.ToString(),

                })
            };
            await _producer.ProduceAsync(_kafkaConfig.TopicName, message);
        }
    }
}
