using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisCache<T> : ICache<T> where T: class
    {
        private readonly IDatabase _db;

        public RedisCache(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisConfig> options)
        {
            var currentConfig = options.Value.GetType().GetProperty(typeof(T).Name).GetValue(options.Value, null) as RedisItemConfig;
            _db = connectionMultiplexer.GetDatabase(currentConfig.DatabaseNumber);

        }
        public async Task<T> GetAsync(string key)
        {
            var hashValue = await _db.StringGetAsync(key);
            if (!hashValue.HasValue)
                return null;
            return JsonConvert.DeserializeObject<T>(hashValue);
        }
    }
}
