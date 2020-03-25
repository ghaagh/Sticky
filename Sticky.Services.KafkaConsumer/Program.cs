using Aerospike.Client;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Advertisement.Implementions;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Services.KafkaConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var setting = configuration.Get<KafkaConsumerSetting>();
            DoJob(setting).GetAwaiter().GetResult();
        }
        public static async Task DoJob(KafkaConsumerSetting setting)
        {
            var services = InjectService(setting);
            var _client = services.GetService<IAsyncClient>();
            var segmentCache = services.GetService<ISegmentCache>();

            var conf = new ConsumerConfig
            {
                GroupId = "default",
                BootstrapServers = $"{setting.KafkaAddress}:{setting.KafkaPort}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("Sticky");

#pragma warning disable IDE0067 // Dispose objects before losing scope
                CancellationTokenSource cts = new CancellationTokenSource();
#pragma warning restore IDE0067 // Dispose objects before losing scope
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    var counter = 0;
                    var pagedSegments = (await segmentCache.GetAllActiveSegments()).Where(x => x.AudienceId == 1).ToList();

                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            counter++;
                            if (counter > 1000 && counter % 1000 == 0)
                            {
                                pagedSegments = (await segmentCache.GetAllActiveSegments()).Where(x => x.AudienceId == 1 && !string.IsNullOrEmpty(x.AudienceExtra)).ToList();
                            }
                            DruidData data = Newtonsoft.Json.JsonConvert.DeserializeObject<DruidData>(cr.Value);
                            if (data != null)
                            {
                                Key matchKey = new Key("Sticky", "Activity", $"{ data.UserId }_{data.HostId}");
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
                                        while (list.Count() > setting.MaxProductRecordPerUser)
                                        {
                                            list.RemoveAt(0);

                                        }

                                        var bin = new Bin(data.StatType, string.Join(",", list));

                                        _client.Put(new WritePolicy(), matchKey, bin);

                                    }
                                    Console.WriteLine(data.UserId);

                                }
                                else if (data.UserId == "PageView")
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
                                        if (data.PageAddress != null && data.PageAddress.Contains(item.AudienceExtra) && !pageSegmentIds.Contains(item.Id.ToString()))
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
        public static ServiceProvider InjectService(KafkaConsumerSetting setting)
        {

          
            var serviceProvider = new ServiceCollection().AddSingleton<IAsyncClient, AsyncClient>(c => new AsyncClient(setting.AerospikeAddress, setting.AerospikePort))
                                                         .AddSingleton<IRedisCache, RedisCache>()
                                                         .AddSingleton<ISegmentCache, SegmentCache>()
                                                         .BuildServiceProvider();
                                                
            return serviceProvider;

        }
    }
}
