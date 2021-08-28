using MongoDB.Driver;
using Sticky.Domain.CookieSyncing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Mongo
{
    class MongoCookieSyncRepository : ICookieSyncRepository
    {
        private readonly IMongoDatabase _db;

        public MongoCookieSyncRepository(IMongoClient client)
        {
            _db = client.GetDatabase("Sticky");
        }

        public Task<long> GetStickyCookie(string partnerHash, string partnerUserId)
        {
            throw new System.NotImplementedException();
        }

        public async Task SyncCookieAsync(CookieMatch cookieMatch)
        {

            IMongoCollection<CookieMatch> CookieMatches = _db.GetCollection<CookieMatch>(name: "Partner_" + cookieMatch.PartnerId + "_CookieMatch", settings: new MongoCollectionSettings() { AssignIdOnInsert = true });
            var filter_Builder = Builders<CookieMatch>.Filter;
            var filter = filter_Builder.Eq(c => c.Id, cookieMatch.Id);

            var updates = new List<UpdateDefinition<CookieMatch>>
                {
                    Builders<CookieMatch>.Update.Set(m => m.StickyId, cookieMatch.StickyId),
                    Builders<CookieMatch>.Update.SetOnInsert(m => m.Id, cookieMatch.Id)
                };
            await CookieMatches.UpdateOneAsync(filter, Builders<CookieMatch>.Update.Combine(updates), new UpdateOptions() { IsUpsert = true }).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
