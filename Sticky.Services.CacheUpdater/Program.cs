using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sticky.Repositories.Common.Implementions;
using System.Threading;
using System.Collections.Generic;
using Sticky.Repositories.Common;
using Sticky.Repositories.Advertisement.Implementions;
using Sticky.Repositories.Advertisement;

namespace Sticky.Services.CacheUpdater
{
    public class Program
   {

        static void Main()
        {
            Update().GetAwaiter().GetResult();
        }
        /// <summary>
        /// Updating All Cache Entries into RedisDatabase every 10 minutes.
        /// </summary>
        public async static Task Update()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var connectionString = configuration.GetValue<string>("ConnectionString");
            var druidAddress = configuration.GetValue<string>("DruidClient");
            var interval = configuration.GetValue<int>("UpdateIntervalInMinutes");
            var _redisCache = new RedisCache();
            var serviceProvider = new ServiceCollection().AddSingleton<IRedisCache, RedisCache>(c => _redisCache)
                                                         .AddSingleton<IHostCache, HostCache>().BuildServiceProvider();
            var _hostCache = serviceProvider.GetService<IHostCache>();
            while (true)
            {
                List<Task> tasks = new List<Task>();
                tasks.Add(new SegmentCache(_redisCache).Initial(connectionString));
                tasks.Add(new HostScriptChecker(_redisCache).Initial(connectionString));
                tasks.Add(new HostCache(_redisCache).Initial(connectionString));
                tasks.Add(new PartnerCache(_redisCache).Initial(connectionString));
                tasks.Add(new TotalVisitUpdater(_redisCache, _hostCache).FlushToSql(connectionString));
                tasks.Add(new AwesomeTextGenerator(_redisCache).Initial(connectionString));
                tasks.Add(new CategoryLogger(_redisCache).FlushToSql(connectionString));

                await Task.WhenAll(tasks);
                Thread.Sleep(TimeSpan.FromMinutes(interval));


            }

        }
    }
}
