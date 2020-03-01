using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard
{
    public interface ITextTemplateManager
    {
        Task<bool> UpdateTemplateAsync(UpdateTextTemplateRequestV2 templates);

        Task<List<TextTemplateResult>> GetTemplateAsync(string email, int? id);

    }
}
