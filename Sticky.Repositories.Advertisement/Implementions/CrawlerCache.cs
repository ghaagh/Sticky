using StackExchange.Redis;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class CrowlerCache : ICrowlerCache
    {
        private readonly IRedisCache _redisCache;
        public CrowlerCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task AddUserToCrowlers(long userId)
        {
            IDatabase db = _redisCache.GetDatabase(RedisDatabases.CacheData);
              await db.HashSetAsync("Crowlers", userId,userId);
        }

        public async Task<bool> IsCrowler(long userId)
        {
            IDatabase db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var result = (await db.HashGetAsync("Crowlers", userId));
            return result.HasValue;
        }
    }
}
