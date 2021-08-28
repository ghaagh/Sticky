using Microsoft.EntityFrameworkCore;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.HostAggrigate.Exceptions;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Sql.Repositories
{
    class HostRepository : IHostRepository
    {
        private readonly Context _db;
        public HostRepository(Context db)
        {
            _db = db;
        }
        public async Task<Host> CreateHostAsync(string hostAddress, string userId, ValidityEnum userExpiration = ValidityEnum.Y1, ValidityEnum productExpiration = ValidityEnum.Y1)
        {
            var exist = await _db.Hosts.AnyAsync(c => c.HostAddress.ToLower() == hostAddress.ToLower());
            if (exist)
                throw new DuplicatedHostException();
            var host = new Host(hostAddress, userId, userExpiration, productExpiration);
            await _db.Hosts.AddAsync(host);
            return host;
        }

        public async Task<Host> GetByIdAsync(long id)
        {
            var host = await _db.Hosts.Include(c=>c.UserAccesses).FirstOrDefaultAsync(c => c.Id == id);
            if(host==null)
                throw new System.NotImplementedException();
            return host;
        }
    }
}
