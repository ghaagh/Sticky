using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Sticky.Repositories.Common;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Common.Implementions;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public  class PartnerCache:IPartnerCache
    {
        private readonly IRedisCache _redisCache;
        public PartnerCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
#pragma warning disable CA1822 // Mark members as static
        public async Task Initial(string connectionString)
#pragma warning restore CA1822 // Mark members as static
        {
            DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            using (StickyDbContext _db = new StickyDbContext(optionsBuilder.Options))
            {
            var partners = await _db.Partners.Where(c => c.Verified == true).ToListAsync();
            IDatabase db = new RedisCache().GetDatabase(RedisDatabases.CacheData);
            foreach (var item in partners)
            {
                await  db.HashSetAsync("Partners",item.ParnerHash,Newtonsoft.Json.JsonConvert.SerializeObject(item));
            }
            }

        }
        public async Task<Partner> FindPartner(string hashCode)
        {

            Partner partner = new Partner();
            IDatabase db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var result =(await db.HashGetAsync("Partners",hashCode));
            if(result.HasValue)
            partner = Newtonsoft.Json.JsonConvert.DeserializeObject<Partner>(result);
            return partner;
        }
        public async Task<List<Partner>> ListAsync()
        {

            IDatabase db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var values =await  db.HashValuesAsync("Partners");
            List<Partner> partners = new List<Partner>();
            foreach(var item in values)
            {
                partners.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Partner>(item));
            }
            return partners;
        }
    }
}
