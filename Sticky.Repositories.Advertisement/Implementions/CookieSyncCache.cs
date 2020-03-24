using MongoDB.Driver;
using StackExchange.Redis;
using Sticky.Models.Mongo;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class CookieSyncCache : ICookieSyncCache
    {
        private readonly IRedisCache _redisCache;
        
        private readonly IMongoClient _mongoClient;
        public CookieSyncCache(IRedisCache redisCache,IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _redisCache = redisCache;
        }



        public async Task<long?> FindStickyUserIdAsync(int partnerId, string userId)
        {
            IDatabase db = _redisCache.GetDatabase(RedisDatabases.PartnerCookieMatch);
            var cachedUserId = await db.StringGetAsync($"{partnerId.ToString()}:{userId}");
            if (cachedUserId.HasValue)
                return int.Parse(cachedUserId);
            IMongoDatabase _database = _mongoClient.GetDatabase("TrackEm");
            IMongoCollection<PartnersCookieMatch> CookieMatches = _database.GetCollection<PartnersCookieMatch>(name: "Partner_" + partnerId + "_CookieMatch", settings: new MongoCollectionSettings() { AssignIdOnInsert = true });
            var filter_Builder = Builders<PartnersCookieMatch>.Filter;
            var filter = filter_Builder.Eq(c => c.Id, userId);
            var userData = await CookieMatches.Find(filter).Limit(1).FirstOrDefaultAsync();
            if (userData == null)
                return null;
           await  db.StringSetAsync($"{partnerId.ToString()}:{userId}", userData.StickyId.ToString(),TimeSpan.FromHours(2));
           return userData.StickyId;

        }
    }
}
