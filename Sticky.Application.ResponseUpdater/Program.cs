using Aerospike.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Application.ResponseUpdater.Services;
using Sticky.Domain.ResponseAggrigate;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Redis;
using System.IO;
using System.Threading.Tasks;

namespace Sticky.Application.ResponseUpdater
{
    class Program
    {
        //You should always run at least 3 kinds of response updater one for each type of response.(Category,Product and Query)
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();

        }
        private static async Task MainAsync()
        {
            var setting = GetConfiguration();
            var serviceCollection = InjectSharedDependencies(setting);
            switch (setting.ResponseUpdaterType)
            {
                case ResponseUpdaterTypeEnum.SpecialSegment:
                    serviceCollection
                        .AddSingleton<IAsyncClient, AsyncClient>(c => new AsyncClient(setting.AerospikeClient, 3000))
                        .AddSingleton<IResponseUpdater, QueryResponseUpdater>();
                    break;
                case ResponseUpdaterTypeEnum.Category:
                    serviceCollection
                        .AddSingleton<IResponseUpdater, CategoryResponseUpdater>();
                    break;
                case ResponseUpdaterTypeEnum.ProductAndPage:
                    serviceCollection
                        .AddSingleton<IAsyncClient, AsyncClient>(c => new AsyncClient(setting.AerospikeClient, 3000))
                        .AddSingleton<IResponseUpdater, ProductResponseUpdater>();
                        break;
            }
            var provider = serviceCollection.BuildServiceProvider();
            var responseUpdater = provider.GetService<IResponseUpdater>();
            await responseUpdater.DoAsync();

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
            var collection = new ServiceCollection();
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

            collection.AddSingleton<IMultipleCache<ProductCache>, RedisHashsetCache<ProductCache>>();
            collection.AddSingleton<IMultipleCache<HostCache>, RedisHashsetCache<HostCache>>();

            return collection;
        }

    }
}
