using Microsoft.EntityFrameworkCore;
using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard.Implementions
{
    public class TextTemplateManager : ITextTemplateManager
    {
        private readonly StickyDbContext _db;

        public TextTemplateManager(StickyDbContext db)
        {
            _db = db;
        }


        public async Task<List<TextTemplateResult>> GetTemplateAsync(string email, int? id)
        {
            return await _db.ProductTextTemplates.Where(c => c.SegmentId == id).Select(c => new TextTemplateResult() {Body= c.Template,MaxPrice=c.MaxPrice??0,MinPrice=c.MinPrice??0 }).ToListAsync();

        }


        public async Task<bool> UpdateTemplateAsync(UpdateTextTemplateRequestV2 templates)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.ProductTextTemplates.RemoveRange(_db.ProductTextTemplates.Where(c => c.SegmentId == templates.SegmentId));
                    await _db.SaveChangesAsync();
                    await _db.ProductTextTemplates.AddRangeAsync(templates.Templates.Select(c => new ProductTextTemplate() { SegmentId = templates.SegmentId, Template = c.Body,MinPrice = c.MinPrice,MaxPrice=c.MaxPrice }));
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }
    }
}
