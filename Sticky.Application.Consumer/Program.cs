using Aerospike.Client;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Message;
using Sticky.Infrastructure.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Consumer
{
    class Program
    {
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();

        }
        private static Setting GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            var conf = builder.Build();
            return conf.Get<Setting>();
        }

        private static IServiceCollection InjectSharedDependencies(Setting setting)
        {
            var collection = new ServiceCollection()
            .AddSingleton<IAsyncClient, AsyncClient>(c => new AsyncClient(setting.AerospikeAddress, setting.AerospikePort));
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(setting.Redis);
            collection.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(c => connectionMultiplexer);

            collection.Configure<RedisConfig>(options =>
            {
                options.HostCache = new RedisItemConfig()
                {
                    CacheType = CacheType.Hashset,
                    DatabaseNumber = 0,
                };
                options.SegmentCache = new RedisItemConfig()
                {
                    CacheType = CacheType.Hashset,
                    DatabaseNumber = 0,
                };
                options.ProductCache = new RedisItemConfig()
                {
                    CacheType = CacheType.Key,
                    DatabaseNumber = 1,
                };
                options.PartnerCache = new RedisItemConfig()
                {
                    CacheType = CacheType.Hashset,
                    DatabaseNumber = 0
                };
            });
            collection.AddSingleton<IMultipleCache<HostCache>, RedisHashsetCache<HostCache>>();

            return collection;
        }
        static async Task MainAsync()
        {
            var setting = GetConfiguration();
            var services = InjectSharedDependencies(setting).BuildServiceProvider();
            var _client = services.GetService<IAsyncClient>();
            var segmentCache = services.GetService<IMultipleCache<SegmentCache>>();

            var conf = new ConsumerConfig
            {
                GroupId = "default",
                BootstrapServers = $"{setting.KafkaAddress}:{setting.KafkaPort}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            using var c = new ConsumerBuilder<Ignore, string>(conf).Build();
            c.Subscribe("Sticky");

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
            };

            try
            {

                while (true)
                {
                    try
                    {
                        var cr = c.Consume(cts.Token);
                        var pagedSegments = (await segmentCache.GetListAsync())
                                        .Where(x => x.ActivityType == Domain.SegmentAggrigate.ActivityTypeEnum.VisitPage
                                        && !string.IsNullOrEmpty(x.ActivityExtra)).ToList();

                        MessageModel data = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(cr.Message.Value);
                        if (data != null)
                        {
                            var matchKey = new Key("Sticky", "Activity", $"{ data.UserId }_{data.HostId}");
                            var record = _client.Get(new Policy(), matchKey);
                            if (!string.IsNullOrEmpty(data.ProductId) && data.StatType != "PageView")
                            {

                                if (record == null)
                                {
                                    var bin = new Bin(data.StatType, data.ProductId);
                                    _client.Put(new WritePolicy() { }, matchKey, bin);
                                }
                                else
                                {
                                    var listStr = record.GetString(data.StatType);
                                    listStr += "," + data.ProductId;
                                    var list = listStr.Split(",").ToList();
                                    while (list.Count > setting.MaxProductRecordPerUser)
                                    {
                                        list.RemoveAt(0);

                                    }

                                    var bin = new Bin(data.StatType, string.Join(",", list));

                                    _client.Put(new WritePolicy(), matchKey, bin);

                                }
                                Console.WriteLine(data.UserId);

                            }
                            else if (data.StatType == "PageView")
                            {

                                var pageSegmentIds = new List<string>();
                                if (record != null)
                                {
                                    var membershipString = record.GetString("PagedMemberships");
                                    if (!string.IsNullOrEmpty(membershipString))
                                    {
                                        pageSegmentIds = membershipString.Split(",").ToList();
                                    }

                                }
                                var hostPageSegment = pagedSegments.Where(x => x.HostId.ToString() == data.HostId).ToList();
                                foreach (var item in hostPageSegment)
                                {
                                    if (data.PageAddress != null && data.PageAddress.Contains(item.ActivityExtra) && !pageSegmentIds.Contains(item.Id.ToString()))
                                    {
                                        pageSegmentIds.Add(item.Id.ToString());
                                    }
                                }
                                var bin = new Bin("PagedMemberships", string.Join(",", pageSegmentIds));
                                _client.Put(new WritePolicy(), matchKey, bin);
                            }
                        }




                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                c.Close();
            }
        }
    }
}
