using StackExchange.Redis;
using Sticky.Domain.ClientUsers;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Redis
{
    public class RedisClientUserRepository : IClientUserRepository
    {
        private readonly int _databaseNumber = 0;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisClientUserRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task<long> CreateAsync()
        {
            var db = _connectionMultiplexer.GetDatabase(_databaseNumber);
            return await db.HashIncrementAsync("Users","UniqueCookieNumber");
        }
    }
}
