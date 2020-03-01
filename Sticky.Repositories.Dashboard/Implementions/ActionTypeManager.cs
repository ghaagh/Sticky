using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;

namespace Sticky.Repositories.Dashboard.Implementions
{
    public class ActionTypeManager : IActionsTypeManager
    {
        private readonly StickyDbContext _db;
        public ActionTypeManager(StickyDbContext db)
        {
            _db = db;
        }
        public async Task<List<ActionResult>> GetActionListAsync()
        {
            return await  _db.ActionTypes.Select(c => new ActionResult() { Id = c.Id, Name = c.Name  }).ToListAsync();
        }
    }
}
