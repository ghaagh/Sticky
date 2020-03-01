using Sticky.Models.Etc;
using System;
using System.Threading.Tasks;

namespace Sticky.Repositories.Common.Implementions
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly IRedisCache _redisCache;
        public ErrorLogger(IRedisCache redisCache) {
            _redisCache = redisCache;
        }
        public async Task LogError(string error)
        {
            var logdb = _redisCache.GetDatabase(RedisDatabases.Logs);
            var key = Guid.NewGuid().ToString();
            await logdb.StringSetAsync(key, error);
            await logdb.KeyExpireAsync(key,DateTime.Now.AddMinutes(3));
        }
    }
}
