using StackExchange.Redis;
using Sticky.Domain.ResponseAggrigate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisResponseRepository : IResponseRepositoy
    {
        private const int databaseNumber = 3;
        private readonly IDatabase _db;
        private readonly IDatabase _excludedSegmentDatabase;
        public RedisResponseRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase(databaseNumber);
            _excludedSegmentDatabase = connectionMultiplexer.GetDatabase(6);
        }
        public async Task<IEnumerable<Membership>> GetMembership(ResponseUpdaterTypeEnum responseType, long stickyId)
        {
            return responseType switch
            {
                ResponseUpdaterTypeEnum.ProductAndPage => await GetGeneralUserMembership(stickyId),
                ResponseUpdaterTypeEnum.Category => await GetCategoryMembership(stickyId),
                ResponseUpdaterTypeEnum.SpecialSegment => await GetExcludedSegmentMembership(stickyId),
                _ => throw new Exception("Type is not specified"),
            };
        }
        public async Task<bool> ExistAsync(ResponseUpdaterTypeEnum responseType, long stickyId, string excludedId = "")
        {
            return responseType switch
            {
                ResponseUpdaterTypeEnum.ProductAndPage => await _db.KeyExistsAsync($"Full_General:{stickyId}"),
                ResponseUpdaterTypeEnum.Category => await _db.KeyExistsAsync($"Full_Category:{stickyId}"),
                ResponseUpdaterTypeEnum.SpecialSegment => await _db.KeyExistsAsync($"Full_{excludedId}:{stickyId}"),
                _ => throw new Exception("Type is not specified"),
            };
        }
        public async Task SetMembership(ResponseUpdaterTypeEnum responseType, long stickyId, List<Membership> memberships, int emptyResponseExpireInMunites, int fullResponseExpireInMinute, string excludedId = "")
        {
            switch (responseType)
            {
                case ResponseUpdaterTypeEnum.ProductAndPage:
                    await SetGeneralResponseAsync(stickyId, memberships, emptyResponseExpireInMunites, fullResponseExpireInMinute);
                    break;
                case ResponseUpdaterTypeEnum.Category:
                    await SetCategoryMembershipAsync(stickyId, memberships, emptyResponseExpireInMunites, fullResponseExpireInMinute);
                    break;
                case ResponseUpdaterTypeEnum.SpecialSegment:
                    await SetQueryMembershipAsync(stickyId, excludedId, memberships, emptyResponseExpireInMunites, fullResponseExpireInMinute);
                    break;
                default:
                    throw new Exception("Type is not specified");
            }
        }



        private async Task<IEnumerable<Membership>> GetGeneralUserMembership(long stickyId)
        {
            var redisDruidData = await _db.StringGetAsync("Full_General:" + stickyId.ToString());
            if (redisDruidData.HasValue)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Membership>>(redisDruidData);
            }
            return new List<Membership>();

        }

        private async Task<IEnumerable<Membership>> GetExcludedSegmentMembership(long stickyId)
        {

            //For every response updater that is running for exclusive segment we have to have a key of its segmentId into this database;
            var excludedSegments = await _excludedSegmentDatabase.StringGetAsync("ExcludedSegments");
            var membershipData = new List<Membership>();
            if (excludedSegments.HasValue)
            {
                var excludedIds = excludedSegments.ToString().Split(',');
                foreach (var exid in excludedIds)
                {
                    var specialSegmentData = await _db.StringGetAsync($"Full_{exid}:" + stickyId.ToString());
                    if (specialSegmentData.HasValue)
                    {
                        membershipData.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Membership>>(specialSegmentData));
                    }
                }
            }
            return membershipData;
        }

        private async Task<IEnumerable<Membership>> GetCategoryMembership(long stickyId)
        {

            var redisCategoryData = await _db.StringGetAsync("Full_Category:" + stickyId.ToString());
            if (redisCategoryData.HasValue)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Membership>>(redisCategoryData);

            }
            return new List<Membership>();
        }

        private async Task SetQueryMembershipAsync(long userId, string excludedId, List<Membership> memberships, int emptyResponseExireInMinutes, int fullResponseExpireInMinutes)
        {
            await _db.StringSetAsync($"Full_{excludedId}:{userId}",
                Newtonsoft.Json.JsonConvert.SerializeObject(memberships),
                memberships.Count == 0 ? TimeSpan.FromMinutes(emptyResponseExireInMinutes) : TimeSpan.FromMinutes(fullResponseExpireInMinutes));
        }

        private async Task SetCategoryMembershipAsync(long userId, List<Membership> memberships, int emptyResponseExireInMinutes, int fullResponseExpireInMinutes)
        {
            await _db.StringSetAsync($"Full_Category:{userId}",
                Newtonsoft.Json.JsonConvert.SerializeObject(memberships),
                memberships.Count == 0 ? TimeSpan.FromMinutes(emptyResponseExireInMinutes) : TimeSpan.FromMinutes(fullResponseExpireInMinutes));
        }
        private async Task SetGeneralResponseAsync(long userId, List<Membership> memberships, int emptyResponseExireInMinutes, int fullResponseExpireInMinutes)
        {
            await _db.StringSetAsync($"Full_General:{userId}", Newtonsoft.Json.JsonConvert.SerializeObject(memberships),
                memberships.Count == 0 ? TimeSpan.FromMinutes(emptyResponseExireInMinutes) : TimeSpan.FromMinutes(fullResponseExpireInMinutes));
        }
    }
}
