using System.Collections.Generic;
using System.Threading.Tasks;
using Sticky.Models.Etc;
using Sticky.Models.Redis;
using Sticky.Repositories.Common;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class UserMembershipFinder : IUserMembershipFinder
    {

        private readonly IRedisCache _redisCache;
        private readonly ICookieSyncCache _cookieSyncCache;
        public UserMembershipFinder(ICookieSyncCache cookieSyncCache, IRedisCache redisCache)
        {
            _redisCache = redisCache;
            _cookieSyncCache = cookieSyncCache;
        }

        public async Task<MembershipData> FindMembershipByPartnerUserIdAsync(int partnerId, string userId)
        {
            var stickyUserId = await _cookieSyncCache.FindStickyUserIdAsync(partnerId, userId);
            if (stickyUserId == null)
                return new MembershipData();
            else return await FindMembershipByStickyIdAsync(stickyUserId ?? 0);
        }

        public async Task<MembershipData> FindMembershipByStickyIdAsync(long stickyId)
        {
            var membershipData = new MembershipData
            {
                StickyUserId = stickyId
            };
            var redisDb = _redisCache.GetDatabase(RedisDatabases.UserSegmentsZero);
            //var redisData = await redisDb.StringGetAsync(stickyId.ToString());


            var redisDruidData = await redisDb.StringGetAsync("Full_General:" + stickyId.ToString());
            var redisRequestDb = _redisCache.GetDatabase(RedisDatabases.UserSegmentsRequest);
            //Adding General Segment Membership
            if ( redisDruidData.HasValue)
            {
                membershipData.Segments.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSegment>>(redisDruidData));
            }
            else
            {

               await redisRequestDb.ListLeftPushAsync($"Empty_General", stickyId);
            }
            //Adding Special Segment Membership
            var excludedSegments = await _redisCache.GetDatabase(RedisDatabases.CacheData).StringGetAsync("ExcludedSegments");
            if (excludedSegments.HasValue)
            {
                var excludedIds = excludedSegments.ToString().Split(',');
                foreach(var exid in excludedIds)
                {
                    var specialSegmentData = await redisDb.StringGetAsync($"Full_{exid}:" + stickyId.ToString());
                    if (specialSegmentData.HasValue)
                    {
                        membershipData.Segments.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSegment>>(specialSegmentData));
                    }
                    else
                    {
                    await redisRequestDb.ListLeftPushAsync($"Empty_{exid}", stickyId);

                    }
                }
            }
            //Adding Special Segment Membership
            var redisCategoryData = await redisDb.StringGetAsync("Full_Category:" + stickyId.ToString());
            if (redisCategoryData.HasValue)
            {
                membershipData.Segments.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSegment>>(redisCategoryData));

            }
            else
            {

                await redisRequestDb.ListLeftPushAsync($"Empty_Category", stickyId);
            }
            return membershipData;
        }
    }
}
