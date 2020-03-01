using System.Collections.Generic;
using System.Threading.Tasks;
using Sticky.Dto.Dashboard.Response;

namespace Sticky.Repositories.Dashboard
{
    public interface IAudienceTypeManager
    {
        Task<List<AudienceTypeResult>> GetAudienceListAsync();
    }
}
