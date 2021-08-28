using StackExchange.Redis;
using Sticky.Domain.ResponseAggrigate;
using System;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisRequestRepository : IRequestRepository
    {
        private const int databaseNumber = 3;
        private readonly IDatabase _db;
        public RedisRequestRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase(databaseNumber);
        }

        public async Task<string> GetLast(ResponseUpdaterTypeEnum type, string exludedId="")
        {
            return type switch
            {
                ResponseUpdaterTypeEnum.ProductAndPage => await _db.ListRightPopAsync($"Empty_General"),
                ResponseUpdaterTypeEnum.Category => await _db.ListRightPopAsync($"Empty_Category"),
                ResponseUpdaterTypeEnum.SpecialSegment => await _db.ListRightPopAsync($"Empty_{exludedId}"),
                _ => throw new Exception("Type is not declared"),
            };
        }

        public async Task EnqueRequest(long request, ResponseUpdaterTypeEnum type, string excludedId = "")
        {
            switch (type)
            {
                case ResponseUpdaterTypeEnum.ProductAndPage:
                    await _db.ListLeftPushAsync($"Empty_General", request);
                    break;
                case ResponseUpdaterTypeEnum.Category:
                    await _db.ListLeftPushAsync($"Empty_Category", request);
                    break;
                case ResponseUpdaterTypeEnum.SpecialSegment:
                    await _db.ListLeftPushAsync($"Empty_{excludedId}", request);
                    break;
            }
        }
    }
}
