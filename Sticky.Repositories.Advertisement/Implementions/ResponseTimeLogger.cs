
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class ResponseTimeLogger : IResponseTimeLogger
    {
        private readonly IRedisCache _redisCache;
        public ResponseTimeLogger(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task LogResponseTime( long timePeriod, double counter)
        {
            var categoryDatabase = _redisCache.GetDatabase(RedisDatabases.ResponseTiming);
            var str = timePeriod < 10 ? "10" : timePeriod < 20 ? "20" : timePeriod < 30 ? "30" : timePeriod < 40 ? "40" : timePeriod < 50 ? "50" : "BAAAAAAD";
            await categoryDatabase.HashIncrementAsync("Timing", str, counter);

        }
    }
}
