using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisUpdater<T> : ICacheUpdater<T> where T : CacheModel
    {
        private readonly IDatabase _db;
        private readonly CacheType _cacheType;

        public RedisUpdater(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisConfig> options)
        {
            var currentConfig = options.Value.GetType().GetProperty(typeof(T).Name).GetValue(options.Value, null) as RedisItemConfig;
            _cacheType = currentConfig.CacheType;
            _db = connectionMultiplexer.GetDatabase(currentConfig.DatabaseNumber);

        }

        public async Task UpdateBulkAsync(Dictionary<string, T> cacheList, bool deleteOld = true)
        {
            if (_cacheType == CacheType.Hashset)
            {
                if (deleteOld)
                {
                    await _db.KeyDeleteAsync(typeof(T).Name);
                }
                foreach (var item in cacheList)
                {
                    await _db.HashSetAsync(typeof(T).Name, item.Key, Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
                }
            }
            else
            {
                foreach (var item in cacheList)
                {
                    await _db.KeyDeleteAsync(typeof(T).Name);
                    await _db.StringSetAsync(item.Key, Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
                }

            }

        }

        public async Task UpdateOneAsync(string key, T cacheItem)
        {
            if (_cacheType == CacheType.Hashset)
            {
                await _db.HashSetAsync(typeof(T).Name, key, Newtonsoft.Json.JsonConvert.SerializeObject(cacheItem));
            }
            else
            {
                await _db.StringSetAsync(key, Newtonsoft.Json.JsonConvert.SerializeObject(cacheItem));
            }
        }
    }
}
