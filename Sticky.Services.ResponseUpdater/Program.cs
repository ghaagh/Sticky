using Aerospike.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Models.Redis;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Advertisement.Implementions;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Services.ResponseUpdater
{
    class Program
    {
        static void Main()
        {
            Console.BackgroundColor = System.ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            #region Read Configuraiton File
            var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var appConfig = configuration.Get<ResponseUpdaterSetting>();
            #endregion

            Update(appConfig).GetAwaiter().GetResult();
        }
        public static ServiceProvider InjectService(ResponseUpdaterSetting config)
        {
            var serviceProvider = new ServiceCollection()
    .AddSingleton<ISegmentCache, SegmentCache>()
    .AddSingleton<IRedisCache, RedisCache>()
    .AddSingleton<ICrowlerCache, CrowlerCache>()
    .AddSingleton<IErrorLogger, ErrorLogger>()
    .AddSingleton<IHostCache, HostCache>()
        .AddSingleton<IAsyncClient, AsyncClient>(c => new AsyncClient(config.AerospikeClient, 3000))
    .AddSingleton<ISegmentCache, SegmentCache>().AddSingleton<IProductCache, ProductCache>()
    .AddSingleton<IResultCache, ResultCache>()
    .AddSingleton<IRestClient, RestClient>(c => new RestClient(config.DruidClient))
    .BuildServiceProvider();
            return serviceProvider;

        }
        public async static Task Update(ResponseUpdaterSetting config)
        {
            var serviceProvider = InjectService(config);
            var _resultCache = serviceProvider.GetService<IResultCache>();
            var _crowler = serviceProvider.GetService<ICrowlerCache>();
            var _hostCache = serviceProvider.GetService<IHostCache>();
            var _productCache = serviceProvider.GetService<IProductCache>();
            var _segmentCache = serviceProvider.GetService<ISegmentCache>();
            var _redisCache = serviceProvider.GetService<IRedisCache>();
            var _aeroclient = serviceProvider.GetService<IAsyncClient>();
            var _errorLogger = serviceProvider.GetService<IErrorLogger>();
            var requestDb = _redisCache.GetDatabase(RedisDatabases.UserSegmentsRequest);
            var responseDb = _redisCache.GetDatabase(RedisDatabases.UserSegmentsZero);
            //var staticClient = serviceProvider.GetService<RestClient>()
            long PreparedCount = 0;
            long timeOut = 0;
            var batchSize = config.BatchSize;
            long PreparedCountFull = 0;
            var tasks = new List<Task<RestSharp.IRestResponse<List<DruidData>>>>();
            var segmentId = config.SegmentId;
            var hostId = config.HostId;
            var query = config.Query;
            var stopWatch = new Stopwatch();
            if (config.Query)
            {
                var statType = config.StatType;
                while (true)
                {


                    try
                    {

                        List<string> batchRequestIds = new List<string>();
                        Dictionary<string, List<DruidData>> usersDictionary = new Dictionary<string, List<DruidData>>();
                        while (batchRequestIds.Count <= batchSize)
                        {
                            //var batchItem = await requestDb.ListRightPopAsync($"Segment_{segmentId}_Request");
                            var batchItem = await requestDb.ListRightPopAsync($"Empty_{segmentId}");
                            if (!batchItem.HasValue)
                            {
                                Thread.Sleep(4000);
                                break;
                            }
                            PreparedCount++;
                            if (await _crowler.IsCrowler(long.Parse(batchItem)))
                            {
                                continue;
                            }

                            if (responseDb.KeyExists($"Full_{segmentId}:" + batchItem.ToString()))
                            {

                                continue;
                            }
                            batchRequestIds.Add(batchItem);


                        }
                        if (batchRequestIds.Count == 0)
                        {
                            Thread.Sleep(4000);
                            continue;
                        }
                        stopWatch.Reset();
                        stopWatch.Start();
                        foreach (var item in batchRequestIds)
                        {
                            List<UserSegment> finalSegmentsForRedis = new List<UserSegment>();
                            Key matchKey = new Key("Sticky", "Activity", $"{ item }_{config.HostId}");
                            var record = _aeroclient.Get(new Policy(), matchKey);
                            if (record != null)
                            {

                                UserSegment productVisitSegments = new UserSegment()
                                {
                                    HostId = config.HostId ?? 0,
                                    SegmentId = config.SegmentId ?? 0
                                };
                                var r = record.GetValue(config.StatType);
                                List<string> productIds = new List<string>();
                                if (r != null)
                                {
                                    PreparedCountFull++;
                                    productIds = r.ToString().Split(",").Where(c => c != "").ToList();
                                }
                                if (productIds.Count != 0)
                                {
                                    foreach (var productId in productIds.OrderBy(c => Guid.NewGuid()).Take(5))
                                    {
                                        var productData = await _productCache.FindProduct(config.HostId ?? 0, productId);
                                        if (productData != null && !string.IsNullOrEmpty(productData.ProductName) && !string.IsNullOrEmpty(productData.Url) && !string.IsNullOrEmpty(productData.ImageAddress) && productData.IsAvailable && (productData.Price ?? 0) != 0)
                                            productVisitSegments.Products.Add(new Models.Redis.HostProduct() { Id = productData.Id, ImageAddress = productData.ImageAddress, IsAvailable = productData.IsAvailable, ProductName = productData.ProductName, Url = productData.Url, Price = productData.Price, UpdateDate = productData.UpdateDate });
                                    }
                                    finalSegmentsForRedis.Add(productVisitSegments);
                                }

                            }
                            await responseDb.StringSetAsync($"Full_{config.SegmentId}:" + item.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(finalSegmentsForRedis), finalSegmentsForRedis.Count() == 0 ? TimeSpan.FromMinutes(config.EmptyExpirationInMinutes) : TimeSpan.FromMinutes(config.FullExpirationInMinutes));

                        }

                        Console.Clear();
                        Console.WriteLine($"Segment {segmentId}");
                        Console.WriteLine(stopWatch.Elapsed.TotalSeconds + "<--AvgTime To ms");
                        Console.WriteLine(PreparedCount.ToString("N0") + " <---Total Count" + "\n" + PreparedCountFull.ToString("N0") + " <---Filled Count");
                        Console.WriteLine(timeOut);


                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        Thread.Sleep(TimeSpan.FromSeconds(4));
                    }

                }
            }
            else if (config.Categories)
            {
                var catSegment = (await _segmentCache.GetAllActiveSegments()).Where(c => !string.IsNullOrEmpty(c.AudienceExtra)).ToList();
                var categories = catSegment.Select(c => c.AudienceExtra);
                var categoystring = string.Join(',', categories.Select(c => "'" + c + "'"));

                Dictionary<string, List<DruidData>> usersDictionary = new Dictionary<string, List<DruidData>>();
                while (true)
                {
                    try
                    {
                        List<string> batchRequestIds = new List<string>();
                        if (PreparedCount % 1000 == 0)
                        {
                            catSegment = (await _segmentCache.GetAllActiveSegments()).Where(c => !string.IsNullOrEmpty(c.AudienceExtra)).ToList();
                            categories = catSegment.Select(c => c.AudienceExtra);
                            categoystring = string.Join(',', categories.Select(c => "'" + c + "'"));
                        }
                        while (batchRequestIds.Count <= batchSize)
                        {
                            //var batchItem = await requestDb.ListRightPopAsync($"Segment_{segmentId}_Request");
                            var batchItem = await requestDb.ListRightPopAsync($"Empty_Category");
                            if (!batchItem.HasValue)
                            {
                                Thread.Sleep(4000);
                                break;
                            }
                            if (await _crowler.IsCrowler(long.Parse(batchItem)))
                            {
                                continue;
                            }
                            if (responseDb.KeyExists($"Full_Category:" + batchItem.ToString()))
                            {

                                continue;
                            }
                            batchRequestIds.Add(batchItem);
                            PreparedCount++;

                        }
                        if (batchRequestIds.Count == 0)
                        {
                            Thread.Sleep(4000);
                            continue;
                        }
                        stopWatch.Reset();
                        stopWatch.Start();
                        var restClient = new RestSharp.RestClient(config.DruidClient)
                        {
                            Timeout = 4000
                        };
                        var request3 = new RestSharp.RestRequest("druid/v2/sql", RestSharp.Method.POST);
                        string currentQuery = $"select Distinct(CategoryName),UserId from ProductActions WHERE UserId in ({string.Join(',', batchRequestIds.Select(c => "'" + c + "'"))}) AND CategoryName In({categoystring})  and HostId=17 GROUP BY CategoryName,UserId limit {batchSize * catSegment.Count()}";
                        request3.AddJsonBody(new DruidRequest()
                        {
                            query = currentQuery
                        });
                        var responseList = await restClient.ExecuteTaskAsync<List<DruidData>>(request3);
                        if (responseList.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var grouped = responseList.Data.GroupBy(c => c.UserId);
                            foreach (var item in grouped)
                            {
                                if (item.Count() != 0)
                                    usersDictionary.TryAdd(item.Key, item.ToList());
                            }
                        }
                        foreach (var item in batchRequestIds)
                        {
                            var found = false;
                            List<DruidData> userList = new List<DruidData>();
                            found = usersDictionary.TryGetValue(item, out userList);
                            if (found)
                            {
                                PreparedCountFull++;
                                var cats = userList.Select(c => c.CategoryName);
                                var segmentIds = catSegment.Where(c => cats.Contains(c.AudienceExtra));
                                List<UserSegment> finalSegmentsForRedis = new List<UserSegment>();
                                if (segmentIds != null)
                                {

                                    finalSegmentsForRedis.AddRange(segmentIds.Select(c => new UserSegment() { HostId = config.HostId ?? 0, SegmentId = c.Id }));
                                }
                                await responseDb.StringSetAsync($"Full_Category:" + item.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(finalSegmentsForRedis), TimeSpan.FromMinutes(config.FullExpirationInMinutes));

                            }
                        }
                        stopWatch.Stop();
                        Console.Clear();
                        Console.WriteLine($"Category Segment Creation");
                        Console.WriteLine(stopWatch.Elapsed.TotalSeconds + "<--AvgTime To ms");
                        Console.WriteLine(PreparedCount.ToString("N0") + " <---Total Count" + "\n" + PreparedCountFull.ToString("N0") + " <---Filled Count");

                    }
                    catch
                    {

                    }

                }
            }
            else
            {
                // var excludedSegmentString = await _redisCache.GetDatabase(RedisDatabases.CacheData).StringGetAsync("ExcludedSegments");
                // var excludedSegments = excludedSegmentString.ToString().Split(",").Select(c => int.Parse(c)).ToList();
                var activeSegments = (await _segmentCache.GetAllActiveSegments()).ToList();
                var pagedSegments = activeSegments.Where(c => c.AudienceId == 1).Select(c => c.Id.ToString());
                var productSegment = activeSegments.Where(c => c.AudienceId != 1 && c.AudienceExtra == null).ToList();
                var activeHostIds = productSegment.Select(c => c.HostId).Distinct();
                while (true)
                {
                    if (PreparedCount >= 100 && PreparedCount % 100 == 0)
                    {
                        activeSegments = (await _segmentCache.GetAllActiveSegments()).ToList();
                        pagedSegments = activeSegments.Where(c => c.AudienceId == 1).Select(c => c.Id.ToString());
                        productSegment = activeSegments.Where(c => c.AudienceId != 1 && c.AudienceExtra == null).ToList();
                        activeHostIds = productSegment.Select(c => c.HostId).Distinct();
                    }
                    try
                    {

                        Dictionary<string, List<DruidData>> usersDictionary = new Dictionary<string, List<DruidData>>();
                        var item = await requestDb.ListRightPopAsync($"Empty_General");
                        // var item = await requestDb.ListGetByIndexAsync("Empty_General",0);
                        if (!item.HasValue)
                        {
                            Thread.Sleep(4000);
                            continue;
                        }
                        PreparedCount++;
                        if (await _crowler.IsCrowler(long.Parse(item)))
                        {
                            continue;
                        }

                        if (responseDb.KeyExists($"Full_General:" + item.ToString()))
                        {

                            continue;
                        }
                        stopWatch.Reset();
                        stopWatch.Start();
                        List<UserSegment> finalSegmentsForRedis = new List<UserSegment>();
                        Dictionary<int, Dictionary<string, object>> userData = new Dictionary<int, Dictionary<string, object>>();
                        foreach (var activeHost in activeHostIds)
                        {
                            Key matchKey = new Key("Sticky", "Activity", $"{ item }_{activeHost}");
                            var record = _aeroclient.Get(new Policy(), matchKey);
                            if (record == null)
                                continue;
                            Dictionary<string, object> dic = record.bins;
                            userData.TryAdd(activeHost, dic);


                        }
                        if (userData.Count == 0)
                        {
                            await responseDb.StringSetAsync($"Full_General:" + item.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(finalSegmentsForRedis), TimeSpan.FromMinutes(config.EmptyExpirationInMinutes));
                            continue;

                        }
                        foreach (var segment in productSegment)
                        {
                            var foundinDataDic = false;
                            var userDataforhost = new Dictionary<string, object>();

                            foundinDataDic = userData.TryGetValue(segment.HostId, out userDataforhost);
                            if (!foundinDataDic)
                                continue;
                            var statType = segment.AudienceId == 2 ? StatTypes.ProductView : segment.AudienceId == 3 ? StatTypes.AddToCart : segment.AudienceId == 5 ? StatTypes.ProductPurchase : segment.AudienceId == 6 ? StatTypes.Like : "";
                            if (statType == "")
                                continue;
                            var foundSegmentDatainUserdata = false;
                            var segmentData = new object();
                            foundSegmentDatainUserdata = userDataforhost.TryGetValue(statType, out segmentData);
                            if (!foundSegmentDatainUserdata)
                                continue;
                            List<string> productIds = new List<string>();
                            PreparedCountFull++;

                            UserSegment productVisitSegments = new UserSegment()
                            {
                                HostId = segment.HostId,
                                SegmentId = segment.Id
                            };

                            if (segment.ActionId == 2)
                            {
                                productIds = segmentData.ToString().Split(",").Where(c => c != "").Distinct().ToList();
                                if (productIds.Count != 0)
                                {
                                    foreach (var productId in productIds.OrderBy(c => Guid.NewGuid()).Take(5))
                                    {
                                        var productData = await _productCache.FindProduct(segment.HostId, productId);
                                        if (productData != null && !string.IsNullOrEmpty(productData.ProductName) && !string.IsNullOrEmpty(productData.Url) && !string.IsNullOrEmpty(productData.ImageAddress) && productData.IsAvailable && (productData.Price ?? 0) != 0)
                                            productVisitSegments.Products.Add(new Models.Redis.HostProduct() { Id = productData.Id, ImageAddress = productData.ImageAddress, IsAvailable = productData.IsAvailable, ProductName = productData.ProductName, Url = productData.Url, Price = productData.Price, UpdateDate = productData.UpdateDate });
                                    }

                                }
                            }
                            finalSegmentsForRedis.Add(productVisitSegments);





                        }
                        foreach (var hosts in activeHostIds)
                        {
                            var foundinDataDic = false;
                            var userDataforhost = new Dictionary<string, object>();

                            foundinDataDic = userData.TryGetValue(hosts, out userDataforhost);
                            if (!foundinDataDic)
                                continue;
                            var foundSegmentDatainUserdata = false;
                            var segmentData = new object();
                            foundSegmentDatainUserdata = userDataforhost.TryGetValue("PagedMemberships", out segmentData);
                            if (!foundSegmentDatainUserdata)
                                continue;
                            if (segmentData == null)
                                continue;
                            var segmentMembershipList = segmentData.ToString().Split(",");
                            if (segmentMembershipList != null && segmentMembershipList.Count() == 0)
                                continue;
                            var productVisitSegments = new List<UserSegment>();
                            foreach (var item1 in segmentMembershipList)
                            {
                                if (pagedSegments.Contains(item1))
                                    productVisitSegments.Add(new UserSegment()
                                    {
                                        HostId = hosts,
                                        SegmentId = int.Parse(item1)
                                    });
                            }
                            List<string> membership = new List<string>();
                            finalSegmentsForRedis.AddRange(productVisitSegments);


                        }
                        await responseDb.StringSetAsync($"Full_General:" + item.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(finalSegmentsForRedis), finalSegmentsForRedis.Count() == 0 ? TimeSpan.FromMinutes(config.EmptyExpirationInMinutes) : TimeSpan.FromMinutes(config.FullExpirationInMinutes));


                        Console.Clear();
                        Console.WriteLine($"All Product Segment");
                        Console.WriteLine(stopWatch.Elapsed.TotalSeconds + "<--AvgTime To ms");
                        Console.WriteLine(PreparedCount.ToString("N0") + " <---Total Count" + "\n" + PreparedCountFull.ToString("N0") + " <---Filled Count");
                        Console.WriteLine(timeOut);


                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        Thread.Sleep(TimeSpan.FromSeconds(4));
                    }

                }
            }


        }
    }
}
