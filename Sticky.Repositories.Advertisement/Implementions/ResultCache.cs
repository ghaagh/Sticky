using Sticky.Models.Etc;
using Sticky.Models.Mongo;
using Sticky.Repositories.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class ResultCache:IResultCache
    {
        private readonly IRedisCache _redisCache;
        public ResultCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task<List<HostProduct>> FindResultAsync(string key)
        {
            var database = _redisCache.GetDatabase(RedisDatabases.ResultZero);
            var data = await database.StringGetAsync(key);
           
            if (data.HasValue)
                return ZeroFormatter.ZeroFormatterSerializer.Deserialize<List<HostProduct>>(data);
            else if (!data.HasValue)
                {
                var database2 = _redisCache.GetDatabase(RedisDatabases.EmptyResult);
                await database2.ListLeftPushAsync("EmptyResult",key);
                }
            return new List<HostProduct>() ;
        }
    }
}
