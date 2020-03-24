
using Microsoft.EntityFrameworkCore;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Models.Redis;
using Sticky.Repositories.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class SegmentCache : ISegmentCache
    {
        private readonly IRedisCache _redisCache;
        public SegmentCache(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task<RedisSegment> FindSegmentAsync(int segmentId)
        {
            var database = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var data = await database.HashGetAsync("Segments", segmentId.ToString());
            if (data.HasValue)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<RedisSegment>(data);
            return new RedisSegment();
        }

        public async Task Initial(string connectionString)
        {

            DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            using (StickyDbContext _db = new StickyDbContext(optionsBuilder.Options))
            {
                var segmentDatabase = _redisCache.GetDatabase(RedisDatabases.CacheData);
                await segmentDatabase.KeyDeleteAsync("Segments");

                var SegmentList = await _db.Segments.Where(c => !c.Paused).ToListAsync();
                foreach (var item in SegmentList)
                {
                    await segmentDatabase.HashSetAsync("Segments", item.Id.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(new RedisSegment()
                    {
                        ActionExtra = item.ActionExtra,
                        ActionId = item.ActionId,
                        AudienceId = item.AudienceId,
                        HostId = item.HostId,
                        AudienceExtra = item.AudienceExtra,
                        Id = item.Id,
                    }));
                }
                await _db.SaveChangesAsync();
            }



        }

        public async Task<List<RedisSegment>> GetAllActiveSegments()
        {
            var database = _redisCache.GetDatabase(RedisDatabases.CacheData);
            var data = await database.HashGetAllAsync("Segments");
            var segments = new List<RedisSegment>();
            foreach (var item in data)
            {
                var segment = Newtonsoft.Json.JsonConvert.DeserializeObject<RedisSegment>(item.Value);
                segments.Add(segment);
            }
            return segments;
        }
    }
}
