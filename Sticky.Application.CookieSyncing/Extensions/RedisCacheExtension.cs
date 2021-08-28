using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Redis;

namespace Sticky.Application.CookieSyncing.Extensions
{
    public static class RedisCacheExtensions
    {
        public static IServiceCollection UseRedisCache(this IServiceCollection serviceCollection, string redisConnection)
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnection);
            serviceCollection.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(c => connectionMultiplexer);
            serviceCollection.Configure<RedisConfig>(options => {
                options.PartnerCache = new RedisItemConfig()
                {
                    CacheType = CacheType.Hashset,
                    DatabaseNumber = 0
                };
            });
            serviceCollection.AddSingleton<IMultipleCache<PartnerCache>, RedisHashsetCache<PartnerCache>>();
            return serviceCollection;
        }

    }


}
