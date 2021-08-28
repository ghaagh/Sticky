using Microsoft.EntityFrameworkCore;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.SegmentAggrigate;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Sql.Repositories
{
    class SegmentRepository : ISegmentRepository
    {
        private readonly Context _db;
        public SegmentRepository(Context db)
        {
            _db = db;
        }

        public async Task<Segment> FindByIdAsync(long id)
        {
            return await _db.Segments.Include(c => c.Host).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
