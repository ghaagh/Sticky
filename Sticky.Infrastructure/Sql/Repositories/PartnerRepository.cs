using Microsoft.EntityFrameworkCore;
using Sticky.Domain.PartnerAggrigate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Sql.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly Context _db;
        public PartnerRepository(Context db)
        {
            _db = db;
        }

        public async Task<Partner> CreatePartnerAsync(Partner partner)
        {
            await _db.Partners.AddAsync(partner);
            return partner;
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _db.Partners.ToListAsync();
        }

        public async Task<Partner> GetPartnerAsync(long id)
        {
            return await _db.Partners.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
