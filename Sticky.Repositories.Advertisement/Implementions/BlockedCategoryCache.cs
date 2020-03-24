using StackExchange.Redis;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public  class BlockedCategoryCache : IBlockedCategoryCache
    {

        private readonly IRedisCache _redisCache;
        public BlockedCategoryCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<List<string>> GetBlockedCategoriesForHostAsync(int hostId)
        {
            List<string> blockedCategories = new List<string>();
            IDatabase db = _redisCache.GetDatabase(RedisDatabases.BlockedCategory);
            var result = await db.HashGetAllAsync(hostId.ToString());
            if (result.Any())
            {
                foreach(var resultItem in result)
                {
                    blockedCategories.Add(resultItem.Value.ToString());
                }
            }
            return blockedCategories;
        }
    }
}
