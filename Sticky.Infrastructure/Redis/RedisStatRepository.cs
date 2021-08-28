using StackExchange.Redis;
using Sticky.Domain.StatAggrigate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisStatRepository : IStatRepository
    {
        private readonly int _databaseNumber = 3;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisStatRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task IncreaseStatAsync(StatTypeEnum statTypeEnum, string increaseParameter)
        {
            var db = _connectionMultiplexer.GetDatabase(_databaseNumber);
            await db.HashIncrementAsync(statTypeEnum.ToString(), increaseParameter);
        }
    }
}
