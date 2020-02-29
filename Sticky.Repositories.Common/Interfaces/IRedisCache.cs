using StackExchange.Redis;
using Sticky.Models.Etc;

namespace Sticky.Repositories.Common.Interfaces
{
    public interface IRedisCache
    {
        /// <summary>
        /// returns a Redis Database from changing or getting a value from redis.
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <returns></returns>
        IDatabase GetDatabase(RedisDatabases redisDatabase);
    }
}
