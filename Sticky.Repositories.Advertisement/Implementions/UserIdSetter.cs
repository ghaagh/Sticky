using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class UserIdSetter : IUserIdSetter
    {
        private readonly IRedisCache _redisCache;
        public UserIdSetter(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task<long> GetNewUserIdAsync()
        {
            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            return await db.StringIncrementAsync("UniqueCookieNumber");
        }
    }
}
