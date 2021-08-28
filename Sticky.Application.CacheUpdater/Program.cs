using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Redis;
using Sticky.Infrastructure.Sql;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.CacheUpdater
{
    class Program
    {
        private static string redisConnectionString;
        private static string sqlConnectionString;
        private static int interval;
        private static ServiceProvider serviceProvider;

        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            GetConfiguration();

            serviceProvider = CreateDependencyInjection();


            while (true)
            {

                await UpdateHosts();

                await UpdatePartners();

                await UpdateSegment();

                Thread.Sleep(TimeSpan.FromMinutes(interval));
            }
        }

        private static void GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            sqlConnectionString = configuration.GetConnectionString("Sticky");
            redisConnectionString = configuration.GetConnectionString("Redis");
            interval = configuration.GetValue<int>("UpdateIntervalInMinutes");
        }

        private static ServiceProvider CreateDependencyInjection()
        {
            var collection = new ServiceCollection();

            collection.AddDbContext<Context>(options => options.UseSqlServer(sqlConnectionString));

            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
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

            collection.AddSingleton<ICacheUpdater<HostCache>, RedisUpdater<HostCache>>();
            collection.AddSingleton<ICacheUpdater<PartnerCache>, RedisUpdater<PartnerCache>>();
            collection.AddSingleton<ICacheUpdater<SegmentCache>, RedisUpdater<SegmentCache>>();

            var serviceProvider = collection.BuildServiceProvider();

            return serviceProvider;
        }

        private static async Task UpdateHosts()
        {
            var db = serviceProvider.GetService<Context>();
            var hostCacheUpdater = serviceProvider.GetService<ICacheUpdater<HostCache>>();
            var hosts = await db.Hosts.Where(c => c.HostValidated).ToListAsync();
            var hostData = hosts.ToDictionary(c => c.HostAddress, c => new HostCache() { 
                CategoryValidated = true,
                Id=c.Id,
                Host = c.HostAddress 
            });
            await hostCacheUpdater.UpdateBulkAsync(hostData, true);
            Console.WriteLine($"Host Cache Updated at {DateTime.Now}.");
        }

        private static async Task UpdatePartners()
        {
            var db = serviceProvider.GetService<Context>();
            var hostCacheUpdater = serviceProvider.GetService<ICacheUpdater<PartnerCache>>();
            var partners = await db.Partners.Where(c => c.Verified).ToListAsync();
            var partnersData = partners.ToDictionary(c => c.Id.ToString(), c => new PartnerCache()
            {
                CookieSyncAddress = c.CookieSyncAddress,
                Id = c.Id,
                Verified = true,
                Domain = c.Domain,
                ParnerHash = c.Hash,
                PartnerName = c.Name
            });
            await hostCacheUpdater.UpdateBulkAsync(partnersData, true);
            Console.WriteLine($"Partners Cache Updated at {DateTime.Now}.");
        }

        private static async Task UpdateSegment()
        {
            var db = serviceProvider.GetService<Context>();
            var segmentCacheUpdater = serviceProvider.GetService<ICacheUpdater<SegmentCache>>();
            var segments = await db.Segments.Where(c => !c.Paused).ToListAsync();
            var segmentData = segments.ToDictionary(c => c.Id.ToString(), c => new SegmentCache()
            {
                ActionExtra = c.ActionExtra,
                ActionType = c.Action,
                ActivityType = c.Activity,
                ActivityExtra = c.ActionExtra,
                Paused = false,
                HostId = c.HostId,
                Public = c.IsPublic,
                SegmentName = c.Name
            });
            await segmentCacheUpdater.UpdateBulkAsync(segmentData, true);
            Console.WriteLine($"Segment Cache Updated at {DateTime.Now}.");
        }


    }
}
