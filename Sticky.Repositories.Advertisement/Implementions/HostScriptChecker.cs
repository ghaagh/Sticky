
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class HostScriptChecker : IHostScriptChecker
    {
        private readonly IRedisCache _redisCache;
        public HostScriptChecker(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task UpdateBuyValidation(int hostId)
        {
            await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashSetAsync(hostId.ToString(), "Buy", true);
        }

        public async Task UpdateCartValidation(int hostId)
        {
            await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashSetAsync(hostId.ToString(), "Cart", true);

        }

        public async Task UpdatePageValidation(int hostId)
        {
            await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashSetAsync(hostId.ToString(), "Page", true);

        }

        public async Task UpdateProductValidation(int hostId)
        {
            await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashSetAsync(hostId.ToString(), "Product", true);

        }
        public async Task UpdateFavValidation(int hostId)
        {
            await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashSetAsync(hostId.ToString(), "Fav", true);

        }
        public async Task Initial(string connectionString)
        {
            DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            using (StickyDbContext _db = new StickyDbContext(optionsBuilder.Options))
            {
                var hosts = await _db.Hosts.Where(c => c.HostValidated == true).ToListAsync();
                IDatabase redisdb = new RedisCache().GetDatabase(RedisDatabases.ResponseTiming);

                foreach (var item in hosts)
                {
                    var keyExists = await redisdb.KeyExistsAsync(item.Id.ToString());
                    if (!keyExists)
                        continue;
                    item.PageValidated = (await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashGetAsync(item.Id.ToString(), "Page")) == "1";
                    item.CategoryValidated = (await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashGetAsync(item.Id.ToString(), "Product")) == "1";
                    item.AddToCardValidated = (await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashGetAsync(item.Id.ToString(), "Cart")) == "1";
                    item.FinalizeValidated = (await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashGetAsync(item.Id.ToString(), "Buy")) == "1";
                    item.FavValidated = (await _redisCache.GetDatabase(RedisDatabases.ResponseTiming).HashGetAsync(item.Id.ToString(), "Fav")) == "1";
                }
                await _db.SaveChangesAsync();
            }


        }
    }
}
