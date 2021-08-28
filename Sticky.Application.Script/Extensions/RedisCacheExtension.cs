using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Domain.StatAggrigate;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Redis;

namespace Sticky.Application.Script.Extensions
{
    public static class RedisCacheExtensions
    {
        public static IServiceCollection UseRedisCache(this IServiceCollection serviceCollection, string redisConnection)
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnection);
            serviceCollection.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(c => connectionMultiplexer);
            serviceCollection.Configure<RedisConfig>(options => {
                options.HostCache = new RedisItemConfig()
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

            serviceCollection.AddSingleton<ICache<ProductCache>, RedisCache<ProductCache>>();
            serviceCollection.AddSingleton<ICacheUpdater<ProductCache>, RedisUpdater<ProductCache>>();
            serviceCollection.AddSingleton<IMultipleCache<HostCache>, RedisHashsetCache<HostCache>>();
            serviceCollection.AddSingleton<IMultipleCache<HostCache>, RedisHashsetCache<HostCache>>();
            serviceCollection.AddSingleton<IStatRepository, RedisStatRepository>();
            return serviceCollection;
        }

    }


}
