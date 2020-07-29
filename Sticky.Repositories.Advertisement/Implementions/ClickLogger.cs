using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class ClickLogger : IClickLogger
    {
        private readonly IUtility _utility;
        private readonly IRedisCache _redisCache;
        public ClickLogger(IRedisCache redisCache, IUtility utility)
        {
            _utility = utility;
            _redisCache = redisCache;
        }
        public async Task IncreaseClick(string fullloghash, string uniqueId)
        {
            var cachedDb = _redisCache.GetDatabase(RedisDatabases.CacheData);

            if ((await cachedDb.StringGetAsync("FullClickLog")).HasValue)
            {
                var db = _redisCache.GetDatabase(RedisDatabases.SegmentStats);
                await db.HashIncrementAsync("Clicks", uniqueId, 1);
                await db.HashIncrementAsync("FullClickLog", fullloghash);
            }
        }
        public async Task IncreaseImpression(string winNoticeString)
        {
            if (string.IsNullOrEmpty(winNoticeString))
                return;
            var impressionItems = winNoticeString.Split(",");
            var db = _redisCache.GetDatabase(RedisDatabases.SegmentStats);

            foreach (var uniqueId in impressionItems)
            {
                await db.HashIncrementAsync("Impressions", uniqueId, 1);
            }
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Flush(string connectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //var currentDay = DateTime.Now.Day;
            //var currentMonth = DateTime.Now.Month;
            //var currentYear = DateTime.Now.Year;
            //DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
            //optionsBuilder.UseSqlServer(connectionString);
            //using (StickyDbContext _db = new StickyDbContext(optionsBuilder.Options))
            //{

            //    var redisDb = _redisCache.GetDatabase(RedisDatabases.SegmentStats);
            //    var currentImpresssions = await redisDb.HashGetAllAsync("Impressions");
            //    var currentClicks = await redisDb.HashGetAllAsync("Impressions");
            //    foreach (var item in currentImpresssions)
            //    {
            //        var templateData = _encodeDecodeManager.Base64Decode(item.Name);
            //        var segmentId = int.Parse(templateData.Split("$$$")[0]);
            //        var templateText = templateData.Split("$$$")[1];
            //        var Impressioncounter = int.Parse(item.Value);
            //        var clickCounter = 0;
            //        var clickData = currentClicks.Where(c => c.Name == item.Name);
            //        if (clickData.Count() != 0)
            //            clickCounter = int.Parse(clickData.First().Value);
            //        var findInDictionaryResult = templateDictionary.TryGetValue(templateText, out long templateId);

            //        var currentDayStat = await _db.SegmentStats.FirstOrDefaultAsync(c => c.SegmentId == segmentId && c.Day == currentDay && c.Month == currentMonth && c.Year == currentYear && (!findInDictionaryResult || templateId == c.TextTemplateId));
            //        if (currentDayStat == null)
            //        {
            //            var newDatabaseRow = new SegmentStats() { Clicks = clickCounter, Day = currentDay, Month = currentMonth, Year = currentYear, SegmentId = segmentId, Impressions = Impressioncounter };
            //            if (findInDictionaryResult)
            //                newDatabaseRow.TextTemplateId = (int)templateId;
            //            await _db.SegmentStats.AddAsync(newDatabaseRow);
            //            await _db.SaveChangesAsync();
            //        }
            //        else
            //        {
            //            currentDayStat.Clicks = clickCounter;
            //            currentDayStat.Impressions = Impressioncounter;
            //            await _db.SaveChangesAsync();
            //        }


            //    }
            //}

        }
    }
}
