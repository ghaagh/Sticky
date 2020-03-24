using System.Threading.Tasks;
using Sticky.Models.Etc;
using Sticky.Models.Mongo;
using Sticky.Repositories.Common;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class ProductCache : IProductCache
    {
        private readonly IRedisCache _redisCache;
        public ProductCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task<HostProduct> FindProduct(int hostId, string productId)
        {
            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var current = await db.HashGetAsync("Host_" + hostId.ToString() + "_Products", productId);
            if (current.HasValue)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<HostProduct>(current);
            }
            return new HostProduct();
        }

        public async Task UpdateProduct(int hostId, HostProduct hostProduct)
        {
            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            await db.HashSetAsync("Host_" + hostId.ToString() + "_Products",hostProduct.Id.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(hostProduct));
        }
    }
}
