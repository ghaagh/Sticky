using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;

namespace Sticky.Repositories.Dashboard.Implementions
{
    public class AudienceTypeManager : IAudienceTypeManager
    {
        private readonly StickyDbContext _db;

        public AudienceTypeManager(StickyDbContext db)
        {

            _db = db;
        }

        public async Task<List<AudienceTypeResult>> GetAudienceListAsync()
        {
            return await _db.AudienceTypes.Select(c => new AudienceTypeResult() { Id = c.Id, Name = c.AudienceTypeName }).ToListAsync();
        }
    }
}
