using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sticky.Domain.ResponseAggrigate;
using Sticky.Domain.StatAggrigate;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Infrastructure.Redis;

namespace Sticky.Application.Advertising.Extensions
{
    public static class RedisCacheExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection serviceCollection, string redisConnection)
        {
            serviceCollection.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(c => ConnectionMultiplexer.Connect(redisConnection));
            return serviceCollection;
        }
        public static IServiceCollection AddRedisCache(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMultipleCache<SegmentCache>, RedisHashsetCache<SegmentCache>>();
            return serviceCollection;
        }   
        public static IServiceCollection AddRequestResponseRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRequestRepository,RedisRequestRepository>();
            serviceCollection.AddSingleton<IResponseRepositoy, RedisResponseRepository>();
            return serviceCollection;
        }

    }


}
