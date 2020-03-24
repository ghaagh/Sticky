using Microsoft.EntityFrameworkCore;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Models.Redis;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class HostCache : IHostCache
    {
        private readonly IRedisCache _redisCache;
        public HostCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task<List<Host>> GetListOfHostAsync()
        {

            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var value = await db.HashGetAllAsync("HostId");
            var list = value.Select(c => Newtonsoft.Json.JsonConvert.DeserializeObject<Host>(c.Value));
            return list.ToList();

        }
        public async Task Initial(string connectionString)
        {
            try
            {
                DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                using (StickyDbContext _db = new StickyDbContext(optionsBuilder.Options))
                {
                    var hosts = await _db.Hosts.Where(c => c.HostValidated == true).Select(c => new HostWithoutRelation()
                    {
                        Id = c.Id,
                        AddToCardId = c.AddToCardId,
                        LogoAddress = c.LogoAddress,
                        AddToCardValidated = c.AddToCardValidated,
                        CategoryValidated = c.CategoryValidated,
                        FinalizePage = c.FinalizePage,
                        FinalizeValidated = c.FinalizeValidated,
                        HashCode = c.HashCode,
                        Host = c.HostAddress,
                        ProductImageHeight = c.ProductImageHeight,
                        ProductImageWidth = c.ProductImageWidth,
                        HostPriority = c.HostPriority,
                        HostValidated = c.HostValidated,
                        LogoOtherData = c.LogoOtherData,
                        PageValidated = c.PageValidated,
                        ProductValidated = c.ProductValidated,
                        ProductValidityId = c.ProductValidityId,
                        UserId = c.UserId,
                        UserValidityId = c.UserValidityId,
                        ValidatingHtmlAddress = c.ValidatingHtmlAddress

                    }).ToListAsync();
                    var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
                    await db.KeyDeleteAsync("HostId");
                    await db.KeyDeleteAsync("Address");
                    foreach (var item in hosts)
                    {
                        await db.HashSetAsync("HostId", item.Id.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(item));
                    }
                    foreach (var item in hosts)
                    {
                        await db.HashSetAsync("Address", item.Host.ToLower(), Newtonsoft.Json.JsonConvert.SerializeObject(item));
                    }
                }


            }
            catch
            {

            }

        }
        public bool HostExists(string host)

        {

            var aut = new Uri(host).Host;
            aut = aut.ToLower();
            string topdomain;
            if (aut.IndexOf(".") == aut.LastIndexOf("."))
                topdomain = aut;
            else
                topdomain = aut.Substring(aut.IndexOf(".") + 1);
            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            return db.HashGet("Address", topdomain).HasValue;

        }
        public async Task<HostWithoutRelation> GetHostAsync(string host)

        {
            host = host.ToLower();
            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            HostWithoutRelation current = new HostWithoutRelation();
            var data = await db.HashGetAsync("Address", host);
            if (data.HasValue)
                current = Newtonsoft.Json.JsonConvert.DeserializeObject<HostWithoutRelation>(data);
            return current;
        }

        public async Task<HostWithoutRelation> GetHostByIdAsync(int hostId)
        {

            var db = _redisCache.GetDatabase(RedisDatabases.CacheData);
            HostWithoutRelation current = new HostWithoutRelation();
            var data = await db.HashGetAsync("HostId", hostId);
            if (data.HasValue)
                current = Newtonsoft.Json.JsonConvert.DeserializeObject<HostWithoutRelation>(data);
            return current;
        }
    }
}
