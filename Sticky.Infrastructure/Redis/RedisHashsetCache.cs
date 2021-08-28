using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisHashsetCache<T> : IMultipleCache<T> where T: class
    {
        private readonly IDatabase _db;

        public RedisHashsetCache(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisConfig> options)
        {
            var currentConfig = options.Value.GetType().GetProperty(typeof(T).Name).GetValue(options.Value, null) as RedisItemConfig;
            _db = connectionMultiplexer.GetDatabase(currentConfig.DatabaseNumber);

        }
        
        public async Task<T> GetAsync(string key)
        {
            var hashValue = await _db.HashGetAsync(typeof(T).Name, key);
            if (!hashValue.HasValue)
                return null;
            return JsonConvert.DeserializeObject<T>(hashValue);
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            var hashValue = await _db.HashGetAllAsync(typeof(T).Name);
            if (hashValue.Length==0)
                return null;
            return hashValue.Select(c => JsonConvert.DeserializeObject<T>(c.ToString()));
        }
    }
}
