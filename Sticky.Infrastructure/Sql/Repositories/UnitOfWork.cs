using Sticky.Domain.Shared;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Sql.Repositories
{
    class UnitOfWork:IUnitOfWork
    {
        private readonly Context _db;
        public UnitOfWork(Context db)
        {
            _db = db;
        }

        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
