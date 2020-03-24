using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard
{
    public interface ITextTemplateManager
    {
        Task<bool> UpdateTemplateAsync(UpdateTextTemplateRequest templates);

        Task<List<TextTemplateResult>> GetTemplateAsync(string userId, int? id);

    }
}
